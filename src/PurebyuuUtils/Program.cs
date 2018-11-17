using Purebyuu;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Purebyuu.Output;

namespace PurebyuuUtils
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource(10000);

            var file = new DstFile(File.OpenRead(args[0]));
            await file.Read(cts.Token);

            File.WriteAllText("out.txt", CommandWriter.Write(file), Encoding.UTF8);
            File.WriteAllText("out.svg", SvgWriter.Write(file), Encoding.UTF8);
        }
    }
}
