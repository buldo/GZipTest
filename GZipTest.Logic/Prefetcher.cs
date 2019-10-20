using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GZipTest.Logic
{
    internal class Prefetcher
    {
        private readonly IFileReader _reader;
        private readonly int _batchSize;
        private readonly Thread _readThread;
        private readonly ConcurrentQueue<DataChunk> _chunksQueue = new ConcurrentQueue<DataChunk>();
        private readonly int _toBePrefetched;
        private readonly ManualResetEventSlim _continueRead = new ManualResetEventSlim(true);

        public Prefetcher(IFileReader reader, int prefetchedBatches, int batchSize)
        {
            _reader = reader;
            _batchSize = batchSize;
            _toBePrefetched = prefetchedBatches * batchSize;
            _readThread = new Thread(Read) {IsBackground = true};
            _readThread.Start();
        }

        public event EventHandler<EventArgs> Ended;

        public DataChunk ReadNext()
        {
            if(!_chunksQueue.TryDequeue(out var result))
            {
                result = null;
            }
            _continueRead.Set();
            return result;
        }

        private void Read()
        {
            while (true)
            {
                _continueRead.Wait();
                if (_toBePrefetched - _chunksQueue.Count >= _batchSize)
                {
                    for (int i = 0; i < _batchSize; i++)
                    {
                        var chunk = _reader.ReadNext();
                        if (chunk == null && _chunksQueue.IsEmpty)
                        {
                            OnEnded();
                            return;
                        }

                        _chunksQueue.Enqueue(chunk);
                    }
                }
                else
                {
                    _continueRead.Reset();
                }
            }
        }

        protected virtual void OnEnded()
        {
            Ended?.Invoke(this, EventArgs.Empty);
        }
    }
}
