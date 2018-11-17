using System.Collections.Generic;
using System.Text;

namespace Purebyuu.Extensions
{
    public static class ByteExtensions
    {
        public static string ToHexString(this IEnumerable<byte> ba)
        {
            var hex = new StringBuilder();
            foreach (var b in ba)
                hex.AppendFormat("0x{0:x2} ", b);

            return hex.ToString();
        }

        public static bool IsSet(this byte b, int bit)
        {
            return (b & (1 << bit)) != 0;
        }
    }
}
