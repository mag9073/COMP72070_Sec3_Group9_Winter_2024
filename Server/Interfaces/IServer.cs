using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IServer
    {
        void StartServer(int port);
        void StopServer();
    }
}
