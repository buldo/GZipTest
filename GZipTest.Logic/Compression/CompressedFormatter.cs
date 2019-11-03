using System;
using System.IO;

namespace GZipTest.Logic.Compression
{
    internal class CompressedFormatter : IEncoder
    {
        public void Write(Stream stream, DataChunk chunk)
        {
            stream.Write(BitConverter.GetBytes(chunk.Size));
            stream.Write(chunk.Data, 0, chunk.Size);
        }
    }
}
