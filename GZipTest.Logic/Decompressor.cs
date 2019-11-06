using GZipTest.Logic.Decompression;
using System.IO;

namespace GZipTest.Logic
{
    public class Decompressor
    {
        public void Decompress(string input, string output, int decompressedClunkSize)
        {
            var chunksPool = new DataChunksPool();

            using (var readStream = new FileStream(input, FileMode.Open))
            using (var writeStream = new FileStream(output, FileMode.Create))
            {
                var reader = new CompressedFileReader(readStream, chunksPool.Get);
                var writer = new OrderedWriter(writeStream, new PlainFormatter(), 10, chunksPool.Return);

                var worker = new Worker(
                    reader,
                    writer,
                    new DataChunkDecompressProcessor(decompressedClunkSize, chunksPool.Get),
                    chunksPool.Return);
                worker.Process();
            }
        }
    }
}
