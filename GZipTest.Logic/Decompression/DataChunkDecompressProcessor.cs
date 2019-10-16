using System.IO;
using System.IO.Compression;

namespace GZipTest.Logic.Decompression
{
    internal class DataChunkDecompressProcessor : IDataChunkProcessor
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
