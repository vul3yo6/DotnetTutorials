using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RuntimeLibrariesTests.Models.DependencyInjection;
using System;

namespace RuntimeLibrariesTests
{
    /// <summary>
    /// .NET supports the dependency injection (DI) software design pattern, which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies.
    /// </summary>
    /// <remarks>
    /// Dependency injection in .NET is a built-in part of the framework, along with configuration, logging, and the options pattern.
    /// </remarks>
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection"/>
    public class DependencyInjectionTests
    {
        [Test]
        public void Matcher_IncludePatterns_Case1()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IName, Car>();
            var provider = services.BuildServiceProvider();

            var expected = Car.MyName;

            // Act
            var obj = provider.GetService<IName>();
            var actual = obj.Name;

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Matcher_IncludePatterns_Case2()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IName, Person>();
            var provider = services.BuildServiceProvider();

            var expected = Person.MyName;

            // Act
            var obj = provider.GetService<IName>();
            var actual = obj.Name;

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Matcher_IncludePatterns_Case3()
        {
            // Arrange
            var services = new ServiceCollection();
            //services.AddTransient<IName, Person>(s => new Person());
            services.AddTransient<Car>();
            services.AddTransient<Person>();
            services.AddTransient<Func<NameImplementType, IName>>(service => key =>
            {
                switch (key)
                {
                    case NameImplementType.Car:
                        return service.GetRequiredService<Car>();
                    case NameImplementType.Person:
                        return service.GetRequiredService<Person>();
                    default:
                        throw new NotImplementedException();
                }
            });
            var provider = services.BuildServiceProvider();

            var expected = Person.MyName;

            // Act
            var factory = provider.GetRequiredService<Func<NameImplementType, IName>>();
            var obj = factory.Invoke(NameImplementType.Person);
            var actual = obj.Name;

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
