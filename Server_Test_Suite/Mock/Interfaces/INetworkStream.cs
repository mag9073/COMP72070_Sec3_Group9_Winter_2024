using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Test_Suite.Mock.Interfaces
{
    // https://stackoverflow.com/questions/2176975/how-to-moq-a-networkstream-in-a-unit-test
    public interface INetworkStream
    {
        bool DataAvailable { get; }
        int Read(byte[] buffer, int offset, int size);
        void Write(byte[] buffer, int offset, int size);
        void Flush();
        void Close();
    }
}
