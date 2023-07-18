using AopLib;
using System.Diagnostics;

namespace AopLibTests.Models
{
    internal class EchoAttribute : PolicyInjectionAttributeBase
    {
        public override void BeforeInvoke(object sender, PolicyInjectionAttributeEventArgs e)
        {
            Debug.WriteLine($"{nameof(EchoAttribute)} {nameof(BeforeInvoke)}");
        }

        public override void AfterInvoke(object sender, PolicyInjectionAttributeEventArgs e)
        {
            Debug.WriteLine($"{nameof(EchoAttribute)} {nameof(AfterInvoke)}");
        }

        public override void OnException(object sender, PolicyInjectionAttributeEventArgs e)
        {
            e.ExceptionHandled = true;
            Debug.WriteLine($"{nameof(EchoAttribute)} {nameof(OnException)}");
        }
    }
}
