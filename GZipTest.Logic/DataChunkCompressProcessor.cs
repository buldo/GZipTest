using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GZipTest.Logic
{
    internal class DataChunkCompressProcessor
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

    internal class DataChunkDecompressProcessor
    {
        public void Process(DataChunk chunk, OrderedWriter writer)
        {
            DataChunk processed;
            using (var compressedStream = new MemoryStream(chunk.Data))
            using (var gzStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var decompressedStream = new MemoryStream())
            {
                gzStream.CopyTo(decompressedStream);
                processed = new DataChunk(chunk.Number, decompressedStream.ToArray());
            }

            writer.Append(processed);
        }
    }
}
