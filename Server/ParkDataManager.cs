using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ParkDataManager
    {
        [ProtoContract]
        public class ParkData
        {
            [ProtoMember(1)]
            public string parkName = String.Empty;

            [ProtoMember(2)]
            public string parkAddress = String.Empty;

            [ProtoMember(3)]
            public string parkDescription = String.Empty;

            [ProtoMember(4)]
            public string parkHours = String.Empty;

            public string GetParkName()
            {
                return this.parkName;
            }

            public string GetParkAddress()
            {
                return this.parkAddress;
            }

            public string GetParkDescription()
            {
                return this.parkDescription;
            }

            public string GetParkHours()
            {
                return this.parkHours;
            }

            public void SetParkName(string parkName)
            {
                this.parkName = parkName;
            }

            public void SetParkAddress(string parkAddress)
            {
                this.parkAddress = parkAddress;
            }

            public void SetParkDescription(string parkDescription)
            {
                this.parkDescription = parkDescription;
            }

            public void SetParkHours(string parkHours)
            {
                this.parkHours  = parkHours;
            }

            public byte[] SerializeToByteArray()
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, this);
                    return stream.ToArray();
                }
            }

        }

        // Get all park data from the text file
        // param: takes the file path name as a param 
        // return: ParkData[] - array of Park Data objects
        public static ParkData[] ReadAllParkDataFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            int linesPerPark = 4;
            ParkDataManager.ParkData[] parks = new ParkDataManager.ParkData[lines.Length / linesPerPark];

            for (int i = 0; i < parks.Length; i++)
            {
                int index = i * linesPerPark;
                // this setup is based on the parkdata.txt file
                parks[i] = new ParkDataManager.ParkData
                {
                    parkName = lines[index],
                    parkAddress = lines[index + 1],
                    parkDescription = lines[index + 2],
                    parkHours = lines[index + 3],
                };
            }
            return parks;
        }

        // Get individual park data from athe text file
        // param: the file path name as a param
        // Return: ParkData - a Park Data oject
        // Should be return ParkData object so it can be serialize to be sent back the client
        public static ParkData? ReadOneParkDataFromFile(string filePath, string parkName)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {

                    // Iterate through the the file and each time 
                    // We look for index:
                    // 1. Park Name:
                    // 2. Address
                    // 3. Rating
                    // 4. Descriptions
                    // 5. Number of ratings

                    while (!streamReader.EndOfStream)
                    {
                        string parkName_line = streamReader.ReadLine();

                        // Read the file -> Filter the the file data -> Look for the park name
                        if (parkName_line == parkName)
                        {
                            // The order of each park -> park name, address, rating, description, number of reviews
                            string parkAddress_line = streamReader.ReadLine();
                            string parkDescriptions_line = streamReader.ReadLine();
                            string parkHours_line = streamReader.ReadLine();

                            return new ParkData
                            {
                                parkName = parkName_line,
                                parkAddress = parkAddress_line,
                                parkDescription = parkDescriptions_line,
                                parkHours = parkHours_line
                            };
                        }
                        // Skip the next 4 lines if the current park name does not match any
                        for (int i = 0; i < 3; i++)
                        {
                            streamReader.ReadLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to read park data from this file /o\\ :o {ex.Message}");
            }

            return null; // Return null if we cant find any matching park name
        }




    }
}
