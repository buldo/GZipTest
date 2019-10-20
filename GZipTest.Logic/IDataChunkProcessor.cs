namespace GZipTest.Logic
{
    internal interface IDataChunkProcessor
    {
        void Process(DataChunk chunk, OrderedWriter writer);
    }
}
