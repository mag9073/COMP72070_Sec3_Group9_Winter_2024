using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LogiPark.MVVM.Model
{
    public class Logger
    {
        private DateTime timeOfTransmission;
        private string logFilePath;

        public Logger(string logFileName)
        {
            logFilePath = logFileName;
        }

        public bool Log(byte[] message)
        {
            timeOfTransmission = getTime();
            string output = Encoding.Default.GetString(message);
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(timeOfTransmission.ToString() + ": " + output);
            }
            return true;
        }

        public bool LogPacket(string direction, Packet pkt)
        {
            timeOfTransmission = getTime();
            string type = pkt.GetPacketHeader().GetType().ToString();
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(timeOfTransmission.ToString() + ": " + direction + ", " + type);
            }
            return true;
        }
        public bool LogResponse(string response)
        {
            timeOfTransmission = getTime();
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(timeOfTransmission.ToString() + ": " + response);
            }
            return true;
        }

        private DateTime getTime()
        {
            return DateTime.Now;
        }
    }
}