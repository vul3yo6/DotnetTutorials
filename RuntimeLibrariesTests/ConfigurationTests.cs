using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RuntimeLibrariesTests.Models.Configuration;
using System.Collections.Generic;
using System.IO;

namespace RuntimeLibrariesTests
{
    /// <summary>
    /// Configuration in .NET is performed using one or more configuration providers. 
    /// </summary>
    /// <remarks>
    /// Configuration providers read configuration data from key-value pairs using various configuration sources:
    /// - Settings files, such as appsettings.json
    /// - Environment variables
    /// - Azure Key Vault
    /// - Azure App Configuration
    /// - Command-line arguments
    /// - Custom providers, installed or created
    /// - Directory files
    /// - In-memory.NET objects
    /// - Third-party providers
    /// </remarks>
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration"/>
    public class ConfigurationTests
    {
        // https://stackoverflow.com/questions/837488/how-can-i-get-the-applications-path-in-a-net-console-application
        //private string RootPath { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }
        //private string FilePath { get { return Path.Combine(RootPath, "appsettings.json"); } }
        private string FileName { get { return "appsettings.json"; } }

        [SetUp]
        public void Setup()
        {
            File.WriteAllText(FileName, @"{
    ""Settings"": {
        ""KeyOne"": 1,
        ""KeyTwo"": true,
        ""KeyThree"": {
            ""Message"": ""Oh, that's nice..."",
            ""SupportedVersions"": {
                ""v1"": ""1.0.0"",
                ""v3"": ""3.0.7""
            }
        },
        ""IPAddressRange"": [
            ""46.36.198.121"",
            ""46.36.198.122"",
            ""46.36.198.123"",
            ""46.36.198.124"",
            ""46.36.198.125""
        ]
    }
}");
        }

        [Test]
        public void GetRequiredSection_GetObject_Case1()
        {
            // Arrange
            IConfiguration configRoot = new ConfigurationBuilder()
                .AddJsonFile(FileName, true, true)
                .Build();

            // Get values from the config given their key and their target type.
            // need Microsoft.Extensions.Configuration.Binder
            Settings? settings = configRoot.GetRequiredSection("Settings").Get<Settings>();

            var expected = new List<string>()
            {
                "KeyOne = 1",
                "KeyTwo = True",
                "KeyThree:Message = Oh, that's nice...",
            };

            // Act
            var actual = new List<string>
            {
                $"KeyOne = {settings?.KeyOne}",
                $"KeyTwo = {settings?.KeyTwo}",
                $"KeyThree:Message = {settings?.KeyThree?.Message}"
            };

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetSection_GetObject_Case1()
        {
            // Arrange
            IConfiguration configRoot = new ConfigurationBuilder()
                .AddJsonFile(FileName, true, true)
                .Build();

            Settings? expected = null;

            // Act

            // Get values from the config given their key and their target type.
            // need Microsoft.Extensions.Configuration.Binder
            Settings? actual = configRoot.GetSection("NULL").Get<Settings>();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}