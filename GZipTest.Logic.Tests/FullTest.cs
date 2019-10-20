using NUnit.Framework;
using System.IO;

namespace GZipTest.Logic.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CompressDecompressTest()
        {
            const string inFile = "original.dat";
            if (!File.Exists(inFile))
            {
                DataGenerator.Generate(inFile, 1);
            }

            var compressor = new Compressor();
            const string compressed = "compressed.dat";
            if (!File.Exists(compressed))
            {
                File.Delete(compressed);
            }

            compressor.Compress(inFile, compressed);

            var decomressor = new Decompressor();
            const string decompressed = "decompressed.dat";
            if (!File.Exists(decompressed))
            {
                File.Delete(decompressed);
            }
            decomressor.Decompress(compressed, decompressed);

            FileAssert.AreEqual(inFile, decompressed);
        }
    }
}
