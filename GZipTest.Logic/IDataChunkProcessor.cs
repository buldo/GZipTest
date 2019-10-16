using System;
using System.Collections.Generic;
using System.Text;

namespace GZipTest.Logic
{
    internal interface IDataChunkProcessor
    {
        void Process(DataChunk chunk, OrderedWriter writer);
    }
}
