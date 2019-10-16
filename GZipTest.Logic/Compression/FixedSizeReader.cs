using System;
using System.IO;

namespace GZipTest.Logic.Compression
{
    internal class FixedSizeReader : IFileReader
    {
        private readonly Stream _readStream;
        private readonly int _sizeInBytes;

        private int _cnt = -1;

        public FixedSizeReader(Stream readStream, int sizeInMb)
        {
            _readStream = readStream;
            _sizeInBytes = sizeInMb * 1048576;
        }

        public DataChunk ReadNext()
        {
            var buffer = new byte[_sizeInBytes];
            var readed = _readStream.Read(buffer);
            if (readed == 0)
            {
                return null;
            }

            _cnt++;
            if (readed < buffer.Length)
            {
                var newBuffer = new byte[readed];
                Array.Copy(buffer, 0, newBuffer, 0, readed);
                buffer = newBuffer;
            }

            return new DataChunk(_cnt, buffer);
        }
    }
}
