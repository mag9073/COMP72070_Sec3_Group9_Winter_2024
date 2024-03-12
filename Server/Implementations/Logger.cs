using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implementations
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

        private DateTime getTime()
        {
            return DateTime.Now;
        }
    }
}
