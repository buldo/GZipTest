using System.IO;

namespace GZipTest.Logic
{
    internal interface IEncoder
    {
        void Write(Stream stream, DataChunk chunk);
    }
}