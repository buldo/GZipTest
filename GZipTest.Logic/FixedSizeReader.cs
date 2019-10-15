using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZipTest.Logic
{
    internal class FixedSizeReader : IDisposable
    {
        private readonly FileStream _stream;
        private readonly int _sizeInBytes;

        private int _cnt = -1;

        public FixedSizeReader(string fileName, int sizeInMb)
        {
            _stream = new FileStream(fileName, FileMode.Open);
            _sizeInBytes = sizeInMb * 1048576;
        }

        public DataChunk ReadNext()
        {
            var buffer = new byte[_sizeInBytes];
            var readed = _stream.Read(buffer);
            if(readed == 0)
            {
                return null;
            }

            _cnt++;
            if(readed < buffer.Length)
            {
                var newBuffer = new byte[readed];
                Array.Copy(buffer, 0, newBuffer, 0, readed);
                buffer = newBuffer;
            }

            return new DataChunk(_cnt, buffer);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _stream.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
