using Purebyuu;
using System;
using System.IO;
using System.Text;
using Purebyuu.Output;

namespace PurebyuuUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = new DstFile(File.OpenRead(args[0]));
            file.Read();

            File.WriteAllText("out.txt", CommandWriter.Write(file), Encoding.UTF8);
            File.WriteAllText("out.svg", SvgWriter.Write(file), Encoding.UTF8);
        }
    }
}
