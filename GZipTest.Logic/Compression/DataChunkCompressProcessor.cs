using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;

namespace GZipTest.Logic.Compression
{
    internal class DataChunkCompressProcessor : IDataChunkProcessor
    {
        private readonly Func<int, DataChunk> _chunksAllocator;

        public DataChunkCompressProcessor(
            Func<int, DataChunk> chunksAllocator)
        {
            _chunksAllocator = chunksAllocator;
        }

        public void Process(
            DataChunk originalChunk,
            OrderedWriter writer)
        {
            var processed = _chunksAllocator(originalChunk.Data.Length * 2);
            using (var memoryStream = new MemoryStream(processed.Data))
            {
                using(var gzStream = new GZipStream(memoryStream, CompressionLevel.Optimal, true))
                {
                    gzStream.Write(originalChunk.Data, 0, originalChunk.Size);
                    gzStream.Flush();
                }
                processed.Size = (int)memoryStream.Position;

                processed.Number = originalChunk.Number;
            }

            writer.Append(processed);
        }
    }
}
