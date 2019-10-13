using System;
using System.IO;

namespace GZipTest.Logic.Tests
{
    internal class DataGenerator
    {
        internal static void Generate(string fileName, int sizeInGb)
        {
            var random = new Random();
            var sizeInBytes = sizeInGb * (long)1073741824;
            var written = 0;
            var buffer = new byte[268435456];
            using (var stream = new FileStream(fileName, FileMode.Create))
                while (written < sizeInBytes)
                {
                    random.NextBytes(buffer);
                    stream.Write(buffer);
                    written += buffer.Length;
                }
        }
    }
}