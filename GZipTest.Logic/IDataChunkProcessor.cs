using System.Threading;

namespace GZipTest.Logic
{
    internal interface IDataChunkProcessor
    {
        void Process(DataChunk chunk, OrderedWriter writer, SemaphoreSlim semaphore);
    }
}
