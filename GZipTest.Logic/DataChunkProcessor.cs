using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GZipTest.Logic
{
    internal class DataChunkProcessor
    {
        public void Process(DataChunk chunk, OrderedWriter writer)
        {
            DataChunk processed;
            using(var memoryStream = new MemoryStream())
            using(var gzStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                gzStream.Write(chunk.Data);
                gzStream.Flush();
                processed = new DataChunk(chunk.Number, memoryStream.ToArray());
            }

            writer.Append(processed);
        }
    }
}
