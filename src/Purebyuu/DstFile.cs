using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Purebyuu
{
    public class DstFile
    {
        private readonly Stream _io;

        public DstFile(Stream io)
        {
            _io = io;
        }

        public Header Header { get; private set; }
        public Pattern Body { get; private set; }

        public async Task Read(CancellationToken cancellationToken)
        {
            Body = new Pattern();

            var headerBytes = new byte[512];
            await _io.ReadAsync(headerBytes, 0, 512, cancellationToken);
            Header = new Header(headerBytes);

            var pos = (int)_io.Position;
            var sequinMode = false;
            while (pos < _io.Length)
            {
                var element = new byte[3];
                await _io.ReadAsync(element, 0, 3, cancellationToken);

                if ((element[2] & 0b01000011) == 0b01000011)
                    sequinMode = !sequinMode;
                Body.Add(new Command(element, sequinMode));

                pos += 3;
            }
        }
    }
}
