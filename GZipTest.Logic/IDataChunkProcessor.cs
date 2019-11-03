using System;

namespace GZipTest.Logic
{
    internal interface IDataChunkProcessor
    {
        void Process(DataChunk originalChunk, OrderedWriter writer);
    }
}
