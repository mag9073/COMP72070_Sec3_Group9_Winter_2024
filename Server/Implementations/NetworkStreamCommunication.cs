using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server.Interfaces;

namespace Server.Implementations
{
    public class NetworkStreamCommunication : ICommunicationChannel
    {
        private readonly NetworkStream _stream;

        public NetworkStreamCommunication(NetworkStream stream)
        {
            _stream = stream;
        }

        public void Write(byte[] buffer, int offset, int size) => _stream.Write(buffer, offset, size);

        public async Task WriteAsync(byte[] buffer, int offset, int size) => await _stream.WriteAsync(buffer, offset, size);

        public int Read(byte[] buffer, int offset, int size) => _stream.Read(buffer, offset, size);

        public bool DataAvailable => _stream.DataAvailable;

        public void Close() => _stream.Close();

        public void Flush() => _stream.Flush();
    }
}
