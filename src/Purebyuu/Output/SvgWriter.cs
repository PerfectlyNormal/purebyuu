using System.Linq;
using System.Text;

namespace Purebyuu.Output
{
    /// <summary>
    /// Generate a SVG document representing the pattern
    /// </summary>
    public static class SvgWriter
    {
        public static string Write(DstFile file)
        {
            var extents = file.Body.Extents;
            var width = extents.MaxX - extents.MinX;
            var height = extents.MaxY - extents.MinY;
            var viewBox = $"{extents.MinX} {extents.MinY} {width} {height}";

            var sb = new StringBuilder();
            sb.Append(
                "<?xml version='1.0'?>" +
                $"<svg version='1.1' xmlns='http://www.w3.org/2000/svg' width='{width}' height='{height}' viewBox='{viewBox}'>");

            foreach (var block in file.Body.GetStitchBlocks())
            {
                sb.AppendFormat("<path d='M{0}' fill='none' stroke='#1f1436' stroke-width='3'></path>", string.Join("", block.Select(stitch => $" {stitch.X},{stitch.Y}")));
            }

            sb.Append("</svg>");

            return sb.ToString();
        }
    }
}
