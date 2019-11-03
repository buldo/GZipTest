using System;
using System.IO;

namespace GZipTest.Logic.Decompression
{
    internal class CompressedFileReader : IFileReader
    {
        private int _cnt = -1;

        private readonly Stream _readStream;
        private readonly Func<int, DataChunk> _chunksAllocator;

        public CompressedFileReader(Stream readStream, Func<int, DataChunk> chunksAllocator)
        {
            _readStream = readStream;
            _chunksAllocator = chunksAllocator;
        }

        public DataChunk ReadNext()
        {
            var sizeArray = new byte[4];
            var sizeReaded = _readStream.Read(sizeArray);
            if(sizeReaded != 4)
            {
                return null;
            }

            var size = BitConverter.ToInt32(sizeArray);
            var chunk = _chunksAllocator(size);
            chunk.Size = size;
            var dataReaded = _readStream.Read(chunk.Data, 0, size);
            if (dataReaded != size)
            {
                throw new Exception("Файл повреждён");
            }

            _cnt++;
            chunk.Number = _cnt;
            return chunk;
        }
    }
}
