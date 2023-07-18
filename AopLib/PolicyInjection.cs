namespace AopLib
{
    //此靜態類別用來建構並回傳代理物件
    public static class PolicyInjection
    {
        public static T Create<T>(T instance)
        {
            //建立一個名為 MyProxy 的代理類別，將傳入的 instance 包裹在代理類別中
            T ret = DynamicProxy<T>.Create(instance);
            return ret;
        }
    }
}
