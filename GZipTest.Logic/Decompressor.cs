using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZipTest.Logic
{
    public class Decompressor
    {
        public void Decompress(string input, string output)
        {
            using (var readStream = new FileStream(input, FileMode.Open))
            {
                var reader = new CompressedFileReader(readStream);
                using (var writeStream = new FileStream(output, FileMode.Create))
                {
                    var writer = new OrderedWriter(writeStream, new PlainFormatter());

                    DataChunk chunk = null;
                    var processor = new DataChunkDecompressProcessor();
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
