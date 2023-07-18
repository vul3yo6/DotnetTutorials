using System;

namespace AopLib
{
    public abstract class PolicyInjectionAttributeBase : Attribute
    {
        public ushort Count { get; set; } = 3;
        public ushort Interval { get; set; } = 1000;

        public virtual void BeforeInvoke(object sender, PolicyInjectionAttributeEventArgs e)
        {
        }
        public virtual void AfterInvoke(object sender, PolicyInjectionAttributeEventArgs e)
        {
        }
        public virtual void OnException(object sender, PolicyInjectionAttributeEventArgs e)
        {
        }
    }
}
