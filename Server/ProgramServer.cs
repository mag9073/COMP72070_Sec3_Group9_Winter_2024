using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Implementations;
using ProtoBuf;
using static Server.DataStructure.PacketData;
using Server.Interfaces;
using System.Configuration;

namespace Server
{
    public class ProgramServer
    {
        private static TcpListener _tcpListener;
        private static bool _isRunning;
        private static UserDataManager userDataManager = new UserDataManager();
        private static ParkDataManager parkDataManager = new ParkDataManager();
        private static ParkReviewManager parkReviewManager = new ParkReviewManager();
        private static ServerStateManager serverStateManager = new ServerStateManager();
        private static Logger logger = new Logger("log.txt"); // Adjust the path as necessary
        private static ImageManager imageManager = new ImageManager();
        private static PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);

        static void Main(string[] args)
        {
            IServer server = new Server.Implementations.Server(); 

            server.StartServer(13000);
            Console.WriteLine("Server is running. Press Enter to stop.");
            Console.ReadLine();
            server.StopServer();
        }
    }
}
