using Microsoft.Extensions.FileSystemGlobbing;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RuntimeLibrariesTests
{
    /// <summary>
    /// learn how to use file globbing with the Microsoft.Extensions.FileSystemGlobbing NuGet package.
    /// </summary>
    /// <remarks>
    /// A glob is a term used to define patterns for matching file and directory names based on wildcards. 
    /// Globbing is the act of defining one or more glob patterns, and yielding files from either inclusive or exclusive matches.
    /// </remarks>
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing"/>
    public class FileGlobbingTests
    {
        // https://stackoverflow.com/questions/837488/how-can-i-get-the-applications-path-in-a-net-console-application
        private string RootPath { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }

        [Test]
        public void Matcher_IncludePatterns_Case1()
        {
            // Arrange
            var matcher = new Matcher();
            matcher.AddIncludePatterns(new string[] { "**/*.md", "**/*.mtext" });

            var expected = new List<string>()
            {
                @$"{RootPath}\parent\file.md",
                @$"{RootPath}\parent\README.md",
                @$"{RootPath}\parent\child\file.MD",
                @$"{RootPath}\parent\child\more.md",
                @$"{RootPath}\parent\child\sample.mtext",
                @$"{RootPath}\parent\child\grandchild\file.md",
            };

            // Act
            var actual = matcher.GetResultsInFullPath("parent").ToList();
            
            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Matcher_IncludePatterns_Case2()
        {
            // Arrange
            var matcher = new Matcher();
            matcher.AddInclude("**/assets/**/*");

            var expected = new List<string>()
            {
                @$"{RootPath}\parent\child\assets\image.png",
                @$"{RootPath}\parent\child\assets\image.svg",
            };

            // Act
            var actual = matcher.GetResultsInFullPath("parent").ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Matcher_IncludePatterns_Case3()
        {
            // Arrange
            var matcher = new Matcher();
            matcher.AddInclude("**/*child/**/*");
            matcher.AddExcludePatterns(
                new string[]
                {        
                    "**/*.md", "**/*.text", "**/*.mtext"
                });

            var expected = new List<string>()
            {
                @$"{RootPath}\parent\child\index.js",
                @$"{RootPath}\parent\child\assets\image.png",
                @$"{RootPath}\parent\child\assets\image.svg",
                @$"{RootPath}\parent\child\grandchild\style.css",
            };

            // Act
            var actual = matcher.GetResultsInFullPath("parent").ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}