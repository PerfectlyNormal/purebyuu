using System;
using System.Collections.Generic;
using System.Linq;

namespace Purebyuu
{
    public class Pattern
    {
        private int _previousX;
        private int _previousY;

        public Pattern()
        {
            Stitches = new List<Command>();
            _previousX = 0;
            _previousY = 0;
        }

        public List<Command> Stitches { get; }

        public Extents Extents => new Extents
        {
            MaxX = Stitches.Max(s => s.X),
            MinX = Stitches.Min(s => s.X),
            MaxY = Stitches.Max(s => s.Y),
            MinY = Stitches.Min(s => s.Y)
        };

        /// <summary>
        /// Adds a command relative to the previous location
        /// </summary>
        /// <param name="command"></param>
        public void Add(Command command)
        {
            var x = _previousX + command.X;
            var y = _previousY + command.Y;
            command.X = x;
            command.Y = y;

            AddAbsolute(command);
        }

        public IEnumerable<IEnumerable<Command>> GetStitchBlocks()
        {
            var blocks = new List<List<Command>>();

            var block = new List<Command>();
            blocks.Add(block);
            foreach (var stitch in Stitches)
            {
                if (stitch.CommandType == CommandType.Stitch)
                    block.Add(stitch);
                else
                {
                    block = new List<Command>();
                    blocks.Add(block);
                }
            }

            return blocks.Where(b => b.Count > 0);
        }

        private void AddAbsolute(Command command)
        {
            _previousX = command.X;
            _previousY = command.Y;
            Stitches.Add(command);
        }
    }
}
