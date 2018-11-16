using System;
using System.Linq;
using System.Text;

namespace Purebyuu
{
    public class Header
    {
        public Header(byte[] input)
        {
            var pos = 0;
            while (pos < input.Length)
            {
                var header = Encoding.ASCII.GetString(input.Skip(pos).Take(3).ToArray());
                switch (header)
                {
                    case "LA:": Label             = Encoding.ASCII.GetString(ReadHeader(input, pos, 16)); pos += 3 + 16 + 1; break;
                    case "ST:": Stitches          = Convert.ToInt32(Encoding.ASCII.GetString(ReadHeader(input, pos, 7))); pos += 3 + 7 + 1; break;
                    case "CO:": Colors            = Convert.ToInt32(Encoding.ASCII.GetString(ReadHeader(input, pos, 3))); pos += 3 + 3 + 1; break;
                    case "+X:": XExtents          = Convert.ToInt32(Encoding.ASCII.GetString(ReadHeader(input, pos, 5))); pos += 3 + 5 + 1; break;
                    case "-X:": NegXExtents       = Convert.ToInt32(Encoding.ASCII.GetString(ReadHeader(input, pos, 5))); pos += 3 + 5 + 1; break;
                    case "+Y:": YExtents          = Convert.ToInt32(Encoding.ASCII.GetString(ReadHeader(input, pos, 5))); pos += 3 + 5 + 1; break;
                    case "-Y:": NegYExtents       = Convert.ToInt32(Encoding.ASCII.GetString(ReadHeader(input, pos, 5))); pos += 3 + 5 + 1; break;
                    case "AX:": XDifference       = ReadDiffHeader(input, pos, 6); pos += 3 + 6 + 1; break;
                    case "AY:": YDifference       = ReadDiffHeader(input, pos, 6); pos += 3 + 6 + 1; break;
                    case "MX:": MultiDesignStartX = ReadDiffHeader(input, pos, 6); pos += 3 + 6 + 1; break;
                    case "MY:": MultiDesignStartY = ReadDiffHeader(input, pos, 6); pos += 3 + 6 + 1; break;
                    case "PD:": PreviousDesign    = Encoding.ASCII.GetString(ReadHeader(input, pos, 9)); pos += 3 + 9 + 1; break;
                    default:
                        if (header.Trim() != "")
                            throw new ArgumentOutOfRangeException($"Unknown header element {header}");

                        pos += header.Length;
                        break;
                }
            }
        }

        private static byte[] ReadHeader(byte[] input, int pos, int size)
        {
            var data = input.Skip(pos + 3).Take(size).ToList();
            var index = data.FindIndex(b => b != 0x20);

            data.RemoveRange(0, index);
            return data.ToArray();
        }

        private static int ReadDiffHeader(byte[] input, int pos, int size)
        {
            var data = input.Skip(pos + 3).Take(size).ToList();
            var sign = data.First() == 0x2B ? 1 : -1;

            var index = data.FindIndex(b => b != 0x2B && b != 0x2D && b != 0x20); // Skip sign and spaces
            data.RemoveRange(0, index);

            var val = Convert.ToInt32(Encoding.ASCII.GetString(data.ToArray()));
            return val * sign;
        }

        /// <summary>
        /// The 'LA' entry, which is the design name with no path or extension information.
        /// The blank is 16 characters in total, but the name must not be longer that 8 characters and padded out with 0x20.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Stitch count ST, this is a 7 digit number padded by leading zeros.
        /// This is the total stitch count including color changes, jumps, nups, and special records.
        /// </summary>
        public int Stitches { get; set; }

        /// <summary>
        /// CO or colors, a 3 digit number padded by leading zeros.
        /// This is the number of color change records in the file.
        /// </summary>
        public int Colors { get; set; }

        /// <summary>
        /// Positive X extent in centimeters, a 5 digit non-decimal number padded by leading zeros.
        /// </summary>
        public int XExtents { get; set; }

        /// <summary>
        /// Negative X extent in centimeters, a 5 digit non-decimal number padded by leading zeros.
        /// </summary>
        public int NegXExtents { get; set; }

        /// <summary>
        /// Positive Y extent in centimeters, a 5 digit non-decimal number padded by leading zeros.
        /// </summary>
        public int YExtents { get; set; }

        /// <summary>
        /// Negative Y extent in centimeters, a 5 digit non-decimal number padded by leading zeros.
        /// </summary>
        public int NegYExtents { get; set; }

        /// <summary>
        /// AX should express the relative coordinates of the last point from the start point in 0.1 mm. If the start and last points are the same, the coordinates are (0,0).
        /// </summary>
        public int XDifference { get; set; }

        /// <summary>
        /// AY should express the relative coordinates of the last point from the start point in 0.1 mm. If the start and last points are the same, the coordinates are (0,0).
        /// </summary>
        public int YDifference { get; set; }

        /// <summary>
        /// MX should express coordinates of the last point of the previous file for a multi-volume design.
        /// A multi-volume design means a design consisted of two or more files.
        /// This was used for huge designs that can not be stored in a single paper tape roll.
        /// It is not used so much (almost never) nowadays.
        /// </summary>
        public int MultiDesignStartX { get; set; }

        /// <summary>
        /// MY should express coordinates of the last point of the previous file for a multi-volume design.
        /// A multi-volume design means a design consisted of two or more files.
        /// This was used for huge designs that can not be stored in a single paper tape roll.
        /// It is not used so much (almost never) nowadays.
        /// </summary>
        public int MultiDesignStartY { get; set; }

        /// <summary>
        /// Previous Design (PD). PD is also storing some information for multi-volume design. Modernly this is always "******"
        /// </summary>
        public string PreviousDesign { get; set; }
    }
}
