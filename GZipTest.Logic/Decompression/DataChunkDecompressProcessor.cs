using System.Buffers;
using System.IO;
using System.IO.Compression;

namespace GZipTest.Logic.Decompression
{
    internal class DataChunkDecompressProcessor : IDataChunkProcessor
    {
        private readonly int _decompressedBufferSize;
        private readonly ArrayPool<byte> _pool = ArrayPool<byte>.Shared;

        public DataChunkDecompressProcessor(int decompressedBufferSize)
        {
            _decompressedBufferSize = decompressedBufferSize;
        }

        public void Process(DataChunk chunk, OrderedWriter writer)
        {
            DataChunk processed;
            using (var compressedStream = new MemoryStream(chunk.Data))
            using (var gzStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                var buffer = RentBuffer();
                using (var decompressedStream = new MemoryStream(buffer))
                {
                    gzStream.CopyTo(decompressedStream);

                    var decompressed = new byte[decompressedStream.Position];
                    decompressedStream.Position = 0;
                    decompressedStream.Read(decompressed);
                    processed = new DataChunk(chunk.Number, decompressed);
                }

                _pool.Return(buffer);
            }

            writer.Append(processed);
        }

        private byte[] RentBuffer()
        {
            return _pool.Rent(_decompressedBufferSize);
        }
    }
}
