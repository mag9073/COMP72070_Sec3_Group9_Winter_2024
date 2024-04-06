using ProtoBuf;
using Server.DataStructure;
using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implementations
{
    public class ParkDataManager
    {



        // Get all park data from the text file
        // param: takes the file path name as a param 
        // return: ParkData[] - array of Park Data objects
        public ParkData[] ReadAllParkDataFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            int linesPerPark = 4;
            ParkData[] parks = new ParkData[lines.Length / linesPerPark];

            for (int i = 0; i < parks.Length; i++)
            {
                int index = i * linesPerPark;
                // this setup is based on the parkdata.txt file
                parks[i] = new ParkData
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
        public ParkData? ReadOneParkDataFromFile(string filePath, string parkName)
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



        public void AppendParkDataToFile(string filePath, ParkData parkData)
        {
            try
            {
                StringBuilder parkDataBuffer = new StringBuilder();

                parkDataBuffer.AppendLine(parkData.parkName);
                parkDataBuffer.AppendLine(parkData.parkAddress);
                parkDataBuffer.AppendLine(parkData.parkDescription);
                parkDataBuffer.Append(parkData.parkHours);
                parkDataBuffer.AppendLine();

                File.AppendAllText(filePath, parkDataBuffer.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while attempt to append park data to text file!!! " + e.ToString());
            }

        }

        public void DeleteParkData(string parkName)
        {
            // https://stackoverflow.com/questions/29975219/reading-lines-from-text-file-and-add-to-liststring

            // Read all the lines from the file and store it in the memory as list
            List<string> lines = File.ReadAllLines(Constants.ParkData_FilePath).ToList();

            // We then will use this to hold all the new lines that arent supposed to be deleted
            List<string> remainLines = new List<string>();

            // Iterate through each line looking for matching park name
            for (int i = 0; i < lines.Count; i++)
            {
                // When a park name match is found...
                if (lines[i].Trim().Equals(parkName))
                {
                    // We wanna skip the next 3 lines
                    i = i + 3;
                }
                else
                {
                        // If the current line is not part of the park to delete, we add them back to a line to be written back to our file
                        remainLines.Add(lines[i]);
                }
            }

            // Rewrite the updated park data back to the file
            File.WriteAllLines(Constants.ParkData_FilePath, remainLines.ToArray());

            Console.WriteLine($"{parkName} data deleted successfully.");
        }


        public void EditAParkDataToFile(string filePath, ParkData updatedParkData)
        {
            try
            {

                ParkData[] parks = ReadAllParkDataFromFile(filePath);

                // Find the park to update and modify its information
                for (int i = 0; i < parks.Length; i++)
                {
                    if (parks[i].parkName.Equals(updatedParkData.parkName))
                    {
                        parks[i].parkAddress = updatedParkData.parkAddress;
                        parks[i].parkDescription = updatedParkData.parkDescription;
                        parks[i].parkHours = updatedParkData.parkHours;
                        break;
                    }
                }

                // Rewrite the file with updated parks data
                using (StreamWriter file = new StreamWriter(filePath))
                {
                    foreach (ParkData park in parks)
                    {
                        file.WriteLine(park.parkName);
                        file.WriteLine(park.parkAddress);
                        file.WriteLine(park.parkDescription);
                        file.WriteLine(park.parkHours);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to update the park data: {ex.Message}");
            }
        }

    }
}
