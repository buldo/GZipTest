using System.IO;

namespace GZipTest.Logic.Decompression
{
    internal class PlainFormatter : IEncoder
    {
        public void Write(Stream stream, DataChunk chunk)
        {
            stream.Write(chunk.Data, 0, chunk.Size);
        }
    }
}
