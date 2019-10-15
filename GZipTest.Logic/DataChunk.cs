using System;
using System.Collections.Generic;
using System.Text;

namespace GZipTest.Logic
{
    internal class DataChunk
    {
        public DataChunk(long number, byte[] data)
        {
            Number = number;
            Data = data;
        }

        public long Number { get; }

        public byte[] Data { get; }
    }
}
