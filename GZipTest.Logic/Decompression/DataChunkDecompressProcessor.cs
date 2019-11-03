using System;
using System.IO;
using System.IO.Compression;

namespace GZipTest.Logic.Decompression
{
    internal class DataChunkDecompressProcessor : IDataChunkProcessor
    {
        private readonly int _decompressedBufferSize;
        private readonly Func<int, DataChunk> _chunksAllocator;

        public DataChunkDecompressProcessor(
            int decompressedBufferSize,
            Func<int, DataChunk> chunksAllocator)
        {
            _decompressedBufferSize = decompressedBufferSize;
            _chunksAllocator = chunksAllocator;
        }

        public void Process(DataChunk originalChunk, OrderedWriter writer)
        {
            DataChunk processed;
            using (var compressedStream = new MemoryStream(originalChunk.Data,0,originalChunk.Size))
            using (var gzStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                processed = _chunksAllocator(_decompressedBufferSize);
                using (var decompressedStream = new MemoryStream(processed.Data))
                {
                    gzStream.CopyTo(decompressedStream);
                    processed.Size = (int)decompressedStream.Position; // Вроде не должны выбираться за пределы int
                }
            }

            processed.Number = originalChunk.Number;

            writer.Append(processed);
        }
    }
}
