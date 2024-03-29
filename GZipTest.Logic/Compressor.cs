﻿using GZipTest.Logic.Compression;
using System.IO;

namespace GZipTest.Logic
{
    public class Compressor
    {
        public void Compress(string input, string output, int chunkSize)
        {
            var chunksPool = new DataChunksPool();

            using (var readStream = new FileStream(input, FileMode.Open))
            using (var writeStream = new FileStream(output, FileMode.Create))
            {
                var reader = new FixedSizeReader(readStream, chunkSize, chunksPool.Get);
                var writer = new OrderedWriter(writeStream, new CompressedFormatter(), 10, chunksPool.Return);

                var worker = new Worker(reader, writer, new DataChunkCompressProcessor(chunksPool.Get), chunksPool.Return);
                worker.Process();
            }
        }
    }
}
