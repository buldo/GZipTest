using System.IO;

namespace GZipTest.Logic
{
    public class Compressor
    {
        public void Compress(string input, string output)
        {
            using (var readStream = new FileStream(input, FileMode.Open))
            {
                var reader = new FixedSizeReader(readStream, 1);
                using (var writerStream = new FileStream(output, FileMode.Create))
                {
                    var writer = new OrderedWriter(writerStream, new CompressedFormatter());

                    DataChunk chunk = null;
                    var processor = new DataChunkCompressProcessor();
                    while ((chunk = reader.ReadNext()) != null)
                    {
                        processor.Process(chunk, writer);
                    }
                    writer.Close();
                }
            }
        }
    }
}
