using CommandLine;

namespace GZipTest
{
    [Verb("decompress", HelpText = "Распаковать файл")]
    internal class DecompressConfiguration
    {
        [Value(0, Required = true, HelpText = "Имя исходного файла", MetaName = "input")]
        public string Input { get; set; }

        [Value(1, Required = true, HelpText = "Имя результирующего файла", MetaName = "output")]
        public string Output { get; set; }
    }
}
