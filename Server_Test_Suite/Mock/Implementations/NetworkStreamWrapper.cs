using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_Test_Suite.Mock.Implementations
{
    // https://stackoverflow.com/questions/2176975/how-to-moq-a-networkstream-in-a-unit-test
    public class NetworkStreamWrapper
    {
        private readonly NetworkStream _stream;

        public NetworkStreamWrapper(NetworkStream stream)
        {
            if (stream == null) 
            { 
                throw new ArgumentNullException("stream");
            } 
            _stream = stream;
        }

        public bool DataAvailable
        {
            get
            {
                return _stream.DataAvailable;
            }
        }

        public int Read(byte[] buffer, int offset, int size)
        {
            return _stream.Read(buffer, offset, size);
        }

        public void Write(byte[] buffer, int offset, int size)
        {
            _stream.Write(buffer, offset, size);
        }

        public void Flush()
        {
            _stream.Flush();
        }

        public void Close() => _stream.Close();
    }
}
