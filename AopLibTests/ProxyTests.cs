using AopLib;
using AopLibTests.Models;
using NUnit.Framework;

namespace AopLibTests
{
    public class ProxyTests
    {
        [Test]
        public void Normal_Case1()
        {
            // Arrange
            //var worker = new Lecturer();
            var worker = PolicyInjection.Create<IWorker>(new Lecturer());

            //var expected = new List<string>()
            //{
            //    "KeyOne = 1",
            //    "KeyTwo = True",
            //    "KeyThree:Message = Oh, that's nice...",
            //};

            // Act
            worker.Do();
            //var actual =

            // Assert
            Assert.Pass();
        }

        [Test]
        public void Exception_Case1()
        {
            // Arrange
            //var worker = new ExceptionWorker();
            var worker = PolicyInjection.Create<IWorker>(new ExceptionWorker());

            // Act
            worker.Do();

            // Assert
            Assert.Pass();
        }
    }
}