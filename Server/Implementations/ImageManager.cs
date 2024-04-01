using Server.Interfaces;
using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implementations
{
    public class ImageManager
    {
        public void SaveParkImageToImagesFolder(ICommunicationChannel stream, string parkName)
        {
            string imagePath = Path.Combine(Constants.ParkImages_FilePath, $"{parkName}.jpg");

            // https://learn.microsoft.com/en-us/dotnet/api/system.io.filestream.read?view=net-8.0
            // Create a new file

            using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
            {
                // This is the size of incoming chunk
                byte[] sizeBuffer = new byte[4];

                int bytesToRead;

                // Keep reading til next chunk becomes 0
                while (true)
                {
                    // Read the size of the next chunk
                    bytesToRead = stream.Read(sizeBuffer, 0, sizeBuffer.Length);


                    if (bytesToRead == 0)
                    {
                        throw new Exception("Stream ended prematurely, something went wrong!!!");
                    }

                    // convert byte[] of the size buffet to integer value to know the next incoming chunk size
                    int nextChunkSize = BitConverter.ToInt32(sizeBuffer, 0);

                    if (nextChunkSize == 0)
                    {
                        return;
                    }

                    // We will use this to store buffer based on the incoming chunk size
                    byte[] buffer = new byte[nextChunkSize];

                    // Read the chunk data
                    int totalBytesToRead = 0;

                    // Keep receving it as long the numbe rof total bytes to read is more than incoming chunk
                    while (totalBytesToRead < nextChunkSize)
                    {
                        int readStream = stream.Read(buffer, totalBytesToRead, nextChunkSize - totalBytesToRead);
                        if (readStream == 0)
                        {
                            // This is end of the stream but something went wrong
                            throw new Exception("Stream ended prematurely, something went wrong!!!.");
                        }
                        totalBytesToRead += readStream;
                    }

                    // If we keep writing chunk of buffer to the file chunk at a time
                    fileStream.Write(buffer, 0, totalBytesToRead);
                }
            }
        }
    }
}
