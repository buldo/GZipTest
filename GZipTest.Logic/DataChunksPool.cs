using System.Buffers;
using Microsoft.Extensions.ObjectPool;

namespace GZipTest.Logic
{
    internal class DataChunksPool
    {
        private readonly ArrayPool<byte> _bytesPool = ArrayPool<byte>.Shared;
        private readonly ObjectPool<DataChunk> _chunksPool;

        public DataChunksPool()
        {
            var provider = new DefaultObjectPoolProvider();
            _chunksPool = provider.Create<DataChunk>();
        }

        public DataChunk Get(int dataSize)
        {
            var chunk = _chunksPool.Get();
            chunk.Data = _bytesPool.Rent(dataSize);
            return chunk;
        }

        public void Return(DataChunk chunk)
        {
            var bytes = chunk.Data;
            chunk.Number = 0;
            chunk.Data = null;
            _chunksPool.Return(chunk);
            _bytesPool.Return(bytes);
        }
    }
}
