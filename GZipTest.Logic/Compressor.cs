using System;
using System.IO;
using System.IO.Compression;

namespace GZipTest.Logic
{
    public class Compressor
    {
        public void Compress(string input, string output)
        {
            using(var reader = new FixedSizeReader(input, 1))
            using(var writerStream = new FileStream(output, FileMode.Create))
            {
                var writer = new OrderedWriter(writerStream);

                DataChunk chunk = null;
                var processor = new DataChunkProcessor();
                while((chunk = reader.ReadNext()) != null)
                {
                    processor.Process(chunk, writer);
                }
                writer.Close();
            }
        }
    }
}
