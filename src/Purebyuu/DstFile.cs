using System;
using System.Collections.Generic;
using System.IO;
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

        public void Read()
        {
            var headerBytes = new byte[512];
            _io.Read(headerBytes, 0, 512);
            Header = new Header(headerBytes);
        }
    }
}
