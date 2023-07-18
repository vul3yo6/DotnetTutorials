using System;
using System.Reflection;

namespace AopLib
{
    /// <summary>
    /// PolicyInjectionAttribute 事件參數
    /// </summary>
    public class PolicyInjectionAttributeEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public bool ExceptionHandled { get; set; }
        public object Attribute { get; set; }
        public MethodBase MethodBase { get; set; }
        public object[] MethodArgs { get; set; }
    }
}
