using System;
using System.IO;

namespace GZipTest.Logic
{
    internal class CompressedFormatter : IEncoder
    {
        public void Write(Stream stream, DataChunk chunk)
        {
            stream.Write(BitConverter.GetBytes(chunk.Data.Length));
            stream.Write(chunk.Data);
        }
    }
}
