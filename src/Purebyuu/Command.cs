using System;
using Purebyuu.Extensions;

namespace Purebyuu
{
    /// <summary>
    /// Represents a command to the embroidery machine
    /// </summary>
    /// <list type="table">
    /// DST Encoding
    /// BYTE 	  7 	6 	      5 	  4 	- 	  3 	  2 	  1 	  0
    ///     1 	y+1 	y-1 	y+9 	y-9 	- 	x-9 	x+9 	x-1 	x+1
    ///     2 	y+3 	y-3 	y+27 	y-27 	- 	x-27 	x+27 	x-3 	x+3
    ///     3 	 c0 	 c1     y+81 	y-81 	- 	x-81 	x+81 	set 	set
    /// </list>
    public class Command
    {
        public Command(byte[] input, bool sequinMode)
        {
            if (
                (input.Length == 1 && input[0] == 0b00011010) ||
                (input.Length == 3 && input[0] == 0x1A && input[1] == 0 && input[2] == 0)
                )
            {
                CommandType = CommandType.WilcomEnd;
                return;
            }

            if ((input[2] & 0b00000011) != 0b00000011)
                throw new ArgumentException($"Input {input.ToHexString()} is not a valid command!");

            X = 0;
            Y = 0;

            if (input[2].IsSet(2)) X += 81;
            if (input[2].IsSet(3)) X -= 81;
            if (input[1].IsSet(2)) X += 27;
            if (input[1].IsSet(3)) X -= 27;
            if (input[0].IsSet(2)) X += 9;
            if (input[0].IsSet(3)) X -= 9;
            if (input[1].IsSet(0)) X += 3;
            if (input[1].IsSet(1)) X -= 3;
            if (input[0].IsSet(0)) X += 1;
            if (input[0].IsSet(1)) X -= 1;

            if (input[2].IsSet(5)) Y += 81;
            if (input[2].IsSet(4)) Y -= 81;
            if (input[1].IsSet(5)) Y += 27;
            if (input[1].IsSet(4)) Y -= 27;
            if (input[0].IsSet(5)) Y += 9;
            if (input[0].IsSet(4)) Y -= 9;
            if (input[1].IsSet(7)) Y += 3;
            if (input[1].IsSet(6)) Y -= 3;
            if (input[0].IsSet(7)) Y += 1;
            if (input[0].IsSet(6)) Y -= 1;

            // The Y-axis is flipped, because why not
            Y = -Y;

            if ((input[2] & 0b11110011) == 0b11110011)
                CommandType = CommandType.Stop;
            else if ((input[2] & 0b11000011) == 0b11000011)
                CommandType = CommandType.ColorChange;
            else if ((input[2] & 0b01000011) == 0b01000011)
                CommandType = CommandType.SequinMode;
            else if ((input[2] & 0b10000011) == 0b10000011)
                CommandType = sequinMode ? CommandType.SequinEject : CommandType.Jump;
            else
                CommandType = CommandType.Stitch;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public CommandType CommandType { get; set; }
    }
}
