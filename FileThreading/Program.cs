using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileThreading
{
    public class Program
    {
        private static readonly DirectoryInfo ImportDir = new DirectoryInfo("import");
        private static readonly DirectoryInfo OkDir = new DirectoryInfo("ok");
        private static readonly DirectoryInfo ErrorDir = new DirectoryInfo("error");

        public static void Main(string[] args)
        {

            // Setup test directories
            if (!ImportDir.Exists)
                ImportDir.Create();
            if (!OkDir.Exists)
                OkDir.Create();
            if (!ErrorDir.Exists)
                ErrorDir.Create();

            // Generate test files.
            TestFileGenerator.Execute(ImportDir);
            
            // Create scheduler and task factory
            var scheduler = TaskScheduler.Default;
            var factory = new TaskFactory(scheduler);
            
            // Do the work
            var tasks = new List<Task>();
            foreach (var file in ImportDir.GetFiles())
                tasks.Add(factory.StartNew(() => ProcessFile(file)));

            // Wait for the work to complete
            Task.WaitAll(tasks.ToArray());
        }

        private static void ProcessFile(FileInfo file)
        {
            var fs = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            using (var sr = new StreamReader(fs))
            {
                Console.WriteLine(sr);
            }
            fs.Close();
            file.MoveTo(string.Concat(OkDir.FullName, Path.DirectorySeparatorChar, file.Name));
        }
    }
}
