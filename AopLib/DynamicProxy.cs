using System;
using System.Reflection;
using System.Threading;

namespace AopLib
{
    public class DynamicProxy<T> : DispatchProxy
    {
        private T _targetObject;

        public static T Create(T decorated)
        {
            object proxy = Create<T, DynamicProxy<T>>();
            ((DynamicProxy<T>)proxy)._targetObject = decorated;

            return (T)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            object returnValue = null;

            //檢查該Method身上Attribute中是否有BeforeInvoke Method，如果有，就run他
            CheckIsImplementMethodAndInvoke(targetMethod, "BeforeInvoke", null);
            (int count, int interval) = GetRetryCount(targetMethod);

            do
            {
                try
                {
                    //執行原本的method
                    returnValue = targetMethod.Invoke(_targetObject, args);
                    break;
                }
                catch (Exception ex)
                {
                    count--;

                    // 檢查該Method身上Attribute中是否有OnException Method，如果有，就run他
                    var ret = CheckIsImplementMethodAndInvoke(targetMethod, "OnException", ex);
                    //如果Exception is not handled
                    if (ret == false) throw ex;

                    Thread.Sleep(interval);
                }
            } while (count > 0);

            // 檢查該Method身上Attribute中是否有AfterInvoke Method，如果有，就run他
            CheckIsImplementMethodAndInvoke(targetMethod, "AfterInvoke", null);

            return returnValue;
        }

        //檢查attribute中是否有實作特定的Method，如果有，就run他
        private bool CheckIsImplementMethodAndInvoke(MethodInfo targetMethod, string MethodName, Exception ex)
        {
            //取得包裹的_target object 的類別
            Type type = _targetObject.GetType();

            //尋找該類別的所有方法 
            MethodInfo method = type.GetMethod(targetMethod.Name);

            //檢查有沒有 性能高
            if (method.IsDefined(typeof(PolicyInjectionAttributeBase), true) == false) { return false; }

            //找出該Method身上掛的所有attr
            var attributes = method.GetCustomAttributes(true);

            //在每一個attr上找是否有實作BeforeInvoke的，如果有，就run他
            foreach (var item in attributes)
            {
                var attribute = item.GetType();

                // 還要判斷是否繼承自 PolicyInjectionAttributeBase
                if (attribute.BaseType != typeof(PolicyInjectionAttributeBase)) { continue; }

                //是否有指定的 method
                var attributeMethod = attribute.GetMethod(MethodName);

                //如果有 就 run 他
                if (attributeMethod == null) { continue; }

                //準備參數
                var e = new PolicyInjectionAttributeEventArgs()
                {
                    //MethodArgs = targetMethod.Args,
                    //MethodBase = targetMethod.MethodBase,
                    Attribute = item,
                    Exception = (ex == null ? null : (ex.InnerException != null ? ex.InnerException : ex)),
                    ExceptionHandled = false
                };

                //先提供資訊
                //    ((PolicyInjectionAttributeBase)item).MethodArgs = callMessage.Args;
                //   ((PolicyInjectionAttributeBase)item).MethodBase = callMessage.MethodBase;

                //調用attribute中的 Method
                attributeMethod.Invoke(item, new object[] { targetMethod, e });
                if (e.ExceptionHandled) { return true; }
            }
            return false;
        }

        private Tuple<ushort, ushort> GetRetryCount(MethodInfo targetMethod)
        {
            //取得包裹的_target object 的類別
            Type type = _targetObject.GetType();

            //尋找該類別的所有方法 
            MethodInfo method = type.GetMethod(targetMethod.Name);

            //檢查有沒有 性能高
            if (method.IsDefined(typeof(PolicyInjectionAttributeBase), true) == false) { return Tuple.Create<ushort, ushort>(0, 0); }

            //找出該Method身上掛的所有attr
            var attributes = method.GetCustomAttributes(true);

            //在每一個attr上找是否有實作BeforeInvoke的，如果有，就run他
            foreach (var attribute in attributes)
            {
                var attributeType = attribute.GetType();

                // 還要判斷是否繼承自 PolicyInjectionAttributeBase
                if (attributeType.BaseType != typeof(PolicyInjectionAttributeBase)) { continue; }

                ushort count = GetPropertyValue(attributeType, attribute, "Count");
                ushort interval = GetPropertyValue(attributeType, attribute, "Interval");
                return Tuple.Create<ushort, ushort>(count, interval);
            }

            return Tuple.Create<ushort, ushort>(0, 0);
        }

        private static ushort GetPropertyValue(Type type, object attribute, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null) { return 0; }
            ushort.TryParse(propertyInfo.GetValue(attribute, null).ToString(), out ushort value);
            return value;
        }
    }
}
