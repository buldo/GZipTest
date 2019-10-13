using NUnit.Framework;

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
            DataGenerator.Generate(inFile, 1);
            var compressor = new Compressor();
            const string compressed = "compressed.dat";
            compressor.Compress(inFile, compressed);

            var decomressor = new Decompressor();
            const string decompressed = "decompressed.dat";
            decomressor.Decompress(compressed, decompressed);

            FileAssert.AreEqual(inFile, decompressed);
        }
    }
}