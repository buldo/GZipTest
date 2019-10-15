using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GZipTest.Logic
{
    internal class OrderedWriter
    {
        private readonly SortedSet<DataChunk> _dataChunks;
        private readonly object _bufferLock = new object();
        private readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim(false);
        private readonly Thread _writeThread;
        private readonly Stream _writeStream;
        private readonly IEncoder _formatter;
        private int _lastWritten = -1;
        private bool _isEnded = false;

        public OrderedWriter(Stream stream, IEncoder formatter)
        {
            _writeStream = stream;
            _formatter = formatter;
            _writeThread = new Thread(Write);
            _dataChunks = new SortedSet<DataChunk>(DataChunksComparer.Default);
            _writeThread.Start();
        }

        public void Append(DataChunk chunk)
        {
            lock (_bufferLock)
            {
                _dataChunks.Add(chunk);
            }

            _resetEvent.Set();
        }

        public void Close()
        {
            _isEnded = true;
            _resetEvent.Set();
            _writeThread.Join();
        }

        private void Write()
        {
            while (true)
            {
                _resetEvent.Wait();
                DataChunk next;
                lock (_dataChunks)
                {
                    if (_isEnded && _dataChunks.Count == 0)
                    {
                        break;
                    }

                    if(_dataChunks.Count == 0)
                    {
                        _resetEvent.Reset();
                        continue;
                    }

                    if(_dataChunks.Min.Number != _lastWritten + 1)
                    {
                        _resetEvent.Reset();
                        continue;
                    }

                    next = _dataChunks.Min;
                    _dataChunks.Remove(next);
                }

                _formatter.Write(_writeStream, next);
                _lastWritten++;
            }

            _writeStream.Flush();
        }
    }
}
