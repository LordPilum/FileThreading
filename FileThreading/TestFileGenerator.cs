using System;
using System.IO;

namespace FileThreading
{
    public class TestFileGenerator
    {
        public static void Execute(DirectoryInfo dir)
        {
            var rng = new Random();
            for (var i = 0; i < 100; ++i)
            {
                var path = string.Concat(dir.FullName, Path.DirectorySeparatorChar, Guid.NewGuid(), ".txt");
                using (var sw = File.CreateText(path))
                {
                    var paragraphs = DeFinibusBonorumEtMalorum.GetRandomParagraphs(rng.Next(3, 100));
                    foreach (var paragraph in paragraphs)
                        sw.WriteLine(paragraph);
                }
            }
        }
    }
}
