using System;
using System.Threading;

namespace GZipTest.Logic
{
    internal class Worker
    {
        private readonly OrderedWriter _writer;
        private readonly IDataChunkProcessor _processor;
        private readonly Prefetcher _prefetcher;
        private readonly Thread[] _workerThreads;
        private readonly ManualResetEventSlim _readEndedEvent = new ManualResetEventSlim(false);

        public Worker(
            IFileReader fileReader,
            OrderedWriter writer,
            IDataChunkProcessor processor)
        {
            _prefetcher = new Prefetcher(fileReader, 20, 5);
            _prefetcher.Ended += PrefetcherOnEnded;
            _writer = writer;
            _processor = processor;
            _workerThreads = new Thread[Environment.ProcessorCount];
            for(int i = 0; i < Environment.ProcessorCount; i++)
            {
                _workerThreads[i] = new Thread(ProcessInternal) {IsBackground = true};
            }
        }

        public void Process()
        {
            foreach (var thread in _workerThreads)
            {
                thread.Start();
            }

            _readEndedEvent.Wait();
            foreach (var thread in _workerThreads)
            {
                thread.Join();
            }

            _writer.Close();
        }

        private void ProcessInternal()
        {
            while (!_readEndedEvent.IsSet)
            {
                var chunk = _prefetcher.ReadNext();
                if (chunk == null)
                {
                    continue;
                }

                _processor.Process(chunk, _writer);
            }
        }

        private void PrefetcherOnEnded(object sender, EventArgs e)
        {
            _readEndedEvent.Set();
        }
    }
}
