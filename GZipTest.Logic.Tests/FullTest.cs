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
            const int chunkSize = 1048576;

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

            compressor.Compress(inFile, compressed, chunkSize);

            var decompressor = new Decompressor();
            const string decompressed = "decompressed.dat";
            if (!File.Exists(decompressed))
            {
                File.Delete(decompressed);
            }

            decompressor.Decompress(compressed, decompressed, chunkSize);

            FileAssert.AreEqual(inFile, decompressed);
        }
    }
}
