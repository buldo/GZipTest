using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GZipTest.Logic
{
    internal class DataChunksComparer : IComparer<DataChunk>
    {
        public static IComparer<DataChunk> Default { get; } = new DataChunksComparer();

        public int Compare([AllowNull] DataChunk x, [AllowNull] DataChunk y)
        {
            return x.Number.CompareTo(y.Number);
        }
    }
}
