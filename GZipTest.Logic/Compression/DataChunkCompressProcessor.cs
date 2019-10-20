using System.IO;
using System.IO.Compression;
using System.Threading;

namespace GZipTest.Logic.Compression
{
    internal class DataChunkCompressProcessor : IDataChunkProcessor
    {
        public void Process(DataChunk chunk, OrderedWriter writer, SemaphoreSlim semaphore)
        {
            DataChunk processed;
            using(var memoryStream = new MemoryStream())
            using(var gzStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                gzStream.Write(chunk.Data);
                gzStream.Flush();
                processed = new DataChunk(chunk.Number, memoryStream.ToArray());
            }

            semaphore.Release();

            writer.Append(processed);
        }
    }
}
