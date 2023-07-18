using System;
using System.Diagnostics;

namespace AopLibTests.Models
{
    internal interface IWorker
    {
        void Do();
    }

    internal class Lecturer : IWorker
    {
        [Echo]
        public void Do() 
        {
            Debug.WriteLine("Hello World~!");
        }
    }

    internal class ExceptionWorker : IWorker
    {
        [Echo(Count = 3, Interval = 1000)]
        public void Do()
        {
            Debug.WriteLine("Exception throw");
            throw new Exception("somthing wrong~!");
        }
    }
}
