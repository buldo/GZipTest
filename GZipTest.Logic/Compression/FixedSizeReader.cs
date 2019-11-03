using System;
using System.IO;

namespace GZipTest.Logic.Compression
{
    internal class FixedSizeReader : IFileReader
    {
        private readonly Stream _readStream;
        private readonly int _sizeInBytes;
        private readonly Func<int, DataChunk> _chunksAllocator;

        private int _cnt = -1;

        public FixedSizeReader(Stream readStream, int chunkSize, Func<int, DataChunk> chunksAllocator)
        {
            _readStream = readStream;
            _sizeInBytes = chunkSize;
            _chunksAllocator = chunksAllocator;
        }

        public DataChunk ReadNext()
        {
            var chunk = _chunksAllocator(_sizeInBytes);
            chunk.Size = _readStream.Read(chunk.Data, 0, _sizeInBytes);
            if (chunk.Size == 0)
            {
                return null; // Такое будет происходить в конце файла. Так как у нас консольная утилита, то можем позволить небольшую утечку в конце чтения
            }

            _cnt++;
            chunk.Number = _cnt;

            return chunk;
        }
    }
}
