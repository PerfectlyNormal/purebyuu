using System.Text;

namespace Purebyuu.Output
{
    /// <summary>
    /// Write each command to a string. Helpful for debugging
    /// </summary>
    public static class CommandWriter
    {
        public static string Write(DstFile input)
        {
            var sb = new StringBuilder();

            foreach (var element in input.Body.Stitches)
            {
                var signedX = $"{(element.X >= 0 ? "+" : "")}{element.X}";
                var signedY = $"{(element.Y >= 0 ? "+" : "")}{element.Y}";

                sb.Append($"{element.CommandType}\t{signedX.PadLeft(3)}\t{signedY.PadLeft(3)}\n");
            }

            return sb.ToString();
        }
    }
}
