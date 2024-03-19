using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DotnetLibrariesTests
{
    public class FileSysTests
    {
        [Test]
        public void Test_Case1()
        {
            // Arrange
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dirPath = Path.Combine(rootPath, "App_Data");
            var expected = new List<string>()
            {
                Path.Combine(dirPath, "aabc.txt"), 
                Path.Combine(dirPath, "abc.txt"),
                Path.Combine(dirPath, "abcc.txt"),
            };

            // Act
            var actual = new List<string>();
            foreach (var filePath in Directory.EnumerateFiles(dirPath, "*.txt", SearchOption.TopDirectoryOnly))
            {
                actual.Add(filePath);
            }

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Case2()
        {
            // Arrange
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dirPath = Path.Combine(rootPath, "App_Data");
            var expected = new List<string>()
            {
                Path.Combine(dirPath, "aabc.ini"),
                Path.Combine(dirPath, "aabc.txt"),
                Path.Combine(dirPath, "abc.ini"),
                Path.Combine(dirPath, "abc.txt"),
                Path.Combine(dirPath, "abcc.ini"),
                Path.Combine(dirPath, "abcc.txt"),
            };

            // Act
            var regx = new Regex(@"^(\S+).(txt|ini)$");
            var actual = new List<string>();
            foreach (var filePath in Directory.EnumerateFiles(dirPath, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (regx.IsMatch(filePath))
                {
                    actual.Add(filePath); 
                }
            }

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}