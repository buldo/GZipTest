using System.IO;

namespace GZipTest.Logic
{
    internal class PlainFormatter : IEncoder
    {
        public void Write(Stream stream, DataChunk chunk)
        {
            stream.Write(chunk.Data);
        }
    }
}
