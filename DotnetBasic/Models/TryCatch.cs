using System;

namespace DotnetBasic.Models
{
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/exception-handling-statements
    internal class TryCatch
    {
        public void Run()
        {
            Console.WriteLine("Do_Case1");
            Do_Case1();
            Console.WriteLine("-------------");
            Console.WriteLine("Do_Case2");
            Do_Case2();
            Console.WriteLine("-------------");
            Console.WriteLine("Do_Case3");
            Do_Case3();
        }

        private void Do_Case1()
        {
            try
            {
                Console.WriteLine("do");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }

        private void Do_Case2()
        {
            try
            {
                Console.WriteLine("do");
                throw new Exception("error");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }

        private void Do_Case3()
        {
            try
            {
                Console.WriteLine("do");
                if (true)
                {
                    Console.WriteLine("return");
                    return;
                }
                throw new Exception("error");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }
    }
}
