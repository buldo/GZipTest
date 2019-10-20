using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using GZipTest.Logic;

namespace GZipTest
{
    class Program
    {
        const int ChunkSize = 1048576;

        static int Main(string[] args)
        {
            try
            {
                return Parser.Default.ParseArguments<CompressConfiguration, DecompressConfiguration>(args)
                    .MapResult(
                        (CompressConfiguration opt) => Compress(opt),
                        (DecompressConfiguration opt) => Decompress(opt),
                        errs => 1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Что-то не так:{Environment.NewLine}{e.Message}");
                return 1;
            }
        }

        private static int Compress(CompressConfiguration configuration)
        {
            if (!File.Exists(configuration.Input))
            {
                Console.WriteLine("Входной файл не существует");
                return 1;
            }

            var compressor = new Compressor();
            compressor.Compress(configuration.Input, configuration.Output, ChunkSize);

            return 0;
        }

        private static int Decompress(DecompressConfiguration configuration)
        {
            if (!File.Exists(configuration.Input))
            {
                Console.WriteLine("Входной файл не существует");
                return 1;
            }

            var decompressor = new Decompressor();
            decompressor.Decompress(configuration.Input, configuration.Output, ChunkSize);

            return 0;
        }
    }
}
