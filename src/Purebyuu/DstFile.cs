using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Purebyuu
{
    public class DstFile
    {
        private readonly Stream _io;

        public DstFile(Stream io)
        {
            _io = io;
        }

        public Header Header { get; internal set; }
        public Pattern Body { get; private set; }

        public void Read()
        {
            var headerBytes = new byte[512];
            _io.Read(headerBytes, 0, 512);
            Header = new Header(headerBytes);
            Body = new Pattern();

            var pos = (int)_io.Position;
            var sequinMode = false;
            while (pos < _io.Length)
            {
                var element = new byte[3];
                _io.Read(element, 0, 3);

                if ((element[2] & 0b01000011) == 0b01000011)
                    sequinMode = !sequinMode;
                Body.Add(new Command(element, sequinMode));

                pos += 3;
            }
        }
    }
}
