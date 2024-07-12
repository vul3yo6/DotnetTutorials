using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DotnetLibrariesTests
{
    public class FileSystemWatcherTests
    {
        [Test]
        public void Test1()
        {
            // Arrange
            var expected = "";

            string dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            using var watcher = CreateWatcher(dirPath, "License");
            //using var watcher = CreateWatcher(@"C:\path\to\folder", "*.txt");

            // Act
            var actual = "";
            Thread.Sleep(8000);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private static FileSystemWatcher CreateWatcher(string path, string filter)
        {
            var watcher = new FileSystemWatcher(path);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = filter;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            return watcher;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            // ref: https://stackoverflow.com/questions/1764809/filesystemwatcher-changed-event-is-raised-twice
            var watcher = (FileSystemWatcher)sender;

            try
            {
                watcher.EnableRaisingEvents = false;

                if (e.ChangeType != WatcherChangeTypes.Changed)
                {
                    return;
                }
                Console.WriteLine($"Changed: {e.FullPath}");
            }
            finally
            {
                watcher.EnableRaisingEvents = true;
            }
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
