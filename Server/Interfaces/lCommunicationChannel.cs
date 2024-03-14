using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface ICommunicationChannel
    {
        void Write(byte[] buffer, int offset, int size);
        int Read(byte[] buffer, int offset, int size);
        bool DataAvailable { get; }
        void Close();

        void Flush();
        Task WriteAsync(byte[] nameBytes, int v, int length);
    }
}
