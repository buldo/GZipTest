using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GZipTest.Logic
{
    internal class Worker
    {
        private readonly OrderedWriter _writer;
        private readonly IDataChunkProcessor[] _processors;
        private readonly Prefetcher _prefetcher;
        private readonly ManualResetEventSlim _readEndedEvent = new ManualResetEventSlim(false);

        public Worker(
            IFileReader fileReader,
            OrderedWriter writer,
            Func<IDataChunkProcessor> processorFactory)
        {
            _prefetcher = new Prefetcher(fileReader, 20, 5);
            _prefetcher.Ended += PrefetcherOnEnded;
            _writer = writer;
            _processors = new IDataChunkProcessor[Environment.ProcessorCount];
            for(int i = 0; i < Environment.ProcessorCount; i++)
            {
                _processors[i] = processorFactory();
            }
        }

        public void Process()
        {
            var semaphore = new SemaphoreSlim(_processors.Length, _processors.Length);
            var processor = _processors[0];
            while (!_readEndedEvent.IsSet)
            {
                var chunk = _prefetcher.ReadNext();
                if (chunk == null)
                {
                    continue;
                }
                semaphore.Wait();
                processor.Process(chunk, _writer, semaphore);
            }

            _writer.Close();
        }

        private void ProcessInternal()
        {
            while (true)
            {

            }
        }

        private void PrefetcherOnEnded(object sender, EventArgs e)
        {
            _readEndedEvent.Set();
        }
    }
}
