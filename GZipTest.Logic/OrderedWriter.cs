using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GZipTest.Logic
{
    internal class OrderedWriter
    {
        private readonly SortedSet<DataChunk> _dataChunks;
        private readonly object _bufferLock = new object();
        private readonly ManualResetEventSlim _canWriteEvent = new ManualResetEventSlim(false);
        //private readonly ManualResetEventSlim _canAppendEvent = new ManualResetEventSlim(false);
        private readonly Thread _writeThread;
        private readonly Stream _writeStream;
        private readonly IEncoder _formatter;
        private readonly int _queueLength;
        private readonly Action<DataChunk> _freeChunk;
        private int _lastWritten = -1;
        private volatile bool _isEnded = false;

        public OrderedWriter(
            Stream stream,
            IEncoder formatter,
            int queueLength,
            Action<DataChunk> freeChunk)
        {
            _writeStream = stream;
            _formatter = formatter;
            _queueLength = queueLength;
            _freeChunk = freeChunk;
            _writeThread = new Thread(Write) {IsBackground = true};
            _dataChunks = new SortedSet<DataChunk>(DataChunksComparer.Default);
            _writeThread.Start();
        }

        public void Append(DataChunk chunk)
        {
            while (true)
            {
                if (_lastWritten + 1 == chunk.Number)
                {
                    break;
                }

                int currentCount;
                lock (_bufferLock) // Слишком часто беру локи. Могут быть проблемы
                {
                    currentCount = _dataChunks.Count;
                }

                if (currentCount < _queueLength)
                {
                    break;
                }
            }

            lock (_bufferLock)
            {
                _dataChunks.Add(chunk);
            }

            _canWriteEvent.Set();
        }

        public void Close()
        {
            _isEnded = true;
            _canWriteEvent.Set();
            _writeThread.Join();
        }

        private void Write()
        {
            while (true)
            {
                _canWriteEvent.Wait();
                DataChunk next;
                lock (_bufferLock)
                {
                    if (_isEnded && _dataChunks.Count == 0)
                    {
                        break;
                    }

                    if(_dataChunks.Count == 0)
                    {
                        _canWriteEvent.Reset();
                        continue;
                    }

                    var min = _dataChunks.Min;
                    if (min == null || min.Number != _lastWritten + 1)
                    {
                        _canWriteEvent.Reset();
                        continue;
                    }

                    next = min;
                    _dataChunks.Remove(next);
                }

                _formatter.Write(_writeStream, next);
                _freeChunk(next);
                _lastWritten++;
                //_canAppendEvent.Set();
            }

            _writeStream.Flush();
        }
    }
}
