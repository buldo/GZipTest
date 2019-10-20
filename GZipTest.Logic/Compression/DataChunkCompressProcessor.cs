using System.Buffers;
using System.IO;
using System.IO.Compression;

namespace GZipTest.Logic.Compression
{
    internal class DataChunkCompressProcessor : IDataChunkProcessor
    {
        private readonly ArrayPool<byte> _pool = ArrayPool<byte>.Shared;

        public void Process(DataChunk chunk, OrderedWriter writer)
        {
            var buffer = _pool.Rent(chunk.Data.Length * 2);

            DataChunk processed;
            using (var memoryStream = new MemoryStream(buffer))
            {
                using(var gzStream = new GZipStream(memoryStream, CompressionLevel.Optimal, true))
                {
                    gzStream.Write(chunk.Data);
                    gzStream.Flush();
                }

                var compressed = new byte[memoryStream.Position];
                memoryStream.Position = 0;
                memoryStream.Read(compressed);
                processed = new DataChunk(chunk.Number, compressed);
            }

            _pool.Return(buffer);

            writer.Append(processed);
        }
    }
}
