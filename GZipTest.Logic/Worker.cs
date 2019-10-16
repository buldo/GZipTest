using System;

namespace GZipTest.Logic
{
    internal class Worker
    {
        private readonly IFileReader _fileReader;
        private readonly OrderedWriter _writer;
        private readonly Func<IDataChunkProcessor> _processorFactory;

        public Worker(IFileReader fileReader, OrderedWriter writer, Func<IDataChunkProcessor> processorFactory)
        {
            _fileReader = fileReader;
            _writer = writer;
            _processorFactory = processorFactory;
        }

        public void Process()
        {
            DataChunk chunk = null;
            var processor = _processorFactory();
            while ((chunk = _fileReader.ReadNext()) != null)
            {
                processor.Process(chunk, _writer);
            }

            _writer.Close();
        }
    }
}
