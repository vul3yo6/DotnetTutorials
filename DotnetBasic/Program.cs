using DotnetBasic.Models;
using System;

namespace DotnetBasic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello .NET Core !");

            var obj = new TryCatch();
            obj.Run();

            Console.ReadLine();
        }
    }
}
