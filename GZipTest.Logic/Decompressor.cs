using GZipTest.Logic.Decompression;
using System.IO;

namespace GZipTest.Logic
{
    public class Decompressor
    {
        public void Decompress(string input, string output)
        {
            using (var readStream = new FileStream(input, FileMode.Open))
            using (var writeStream = new FileStream(output, FileMode.Create))
            {
                var reader = new CompressedFileReader(readStream);
                var writer = new OrderedWriter(writeStream, new PlainFormatter());

                var worker = new Worker(reader, writer, new DataChunkDecompressProcessor());
                worker.Process();
            }
        }
    }
}
