using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZipTest.Logic
{
    class CompressedFileReader
    {
        private int _cnt = -1;

        private readonly Stream _readStream;

        public CompressedFileReader(Stream readStream)
        {
            _readStream = readStream;
        }

        public DataChunk ReadNext()
        {
            var sizeArray = new byte[4];
            var readed = _readStream.Read(sizeArray);
            if(readed != 4)
            {
                return null;
            }

            var size = BitConverter.ToInt32(sizeArray);
            var buffer = new byte[size];
            readed = _readStream.Read(buffer);
            if (readed != size)
            {
                throw new Exception("Файл повреждён");
            }

            _cnt++;

            return new DataChunk(_cnt, buffer);
        }
    }
}
