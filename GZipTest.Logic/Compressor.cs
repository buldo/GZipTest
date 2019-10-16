using GZipTest.Logic.Compression;
using System.IO;

namespace GZipTest.Logic
{
    public class Compressor
    {
        public void Compress(string input, string output)
        {
            using (var readStream = new FileStream(input, FileMode.Open))
            using (var writeStream = new FileStream(output, FileMode.Create))
            {
                var reader = new FixedSizeReader(readStream, 1);
                var writer = new OrderedWriter(writeStream, new CompressedFormatter());

                var worker = new Worker(reader, writer, () => new DataChunkCompressProcessor());
                worker.Process();
            }
        }
    }
}
