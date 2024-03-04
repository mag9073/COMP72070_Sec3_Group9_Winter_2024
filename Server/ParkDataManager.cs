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
            public string parkName;

            [ProtoMember(2)]
            public string parkAddress;

            [ProtoMember(3)]
            public float parkReview;

            [ProtoMember(4)]
            public string parkDescription;

            [ProtoMember(5)]
            public uint numberOfReviews;

            public string GetParkName()
            {
                return this.parkName;
            }

            public string GetParkAddress()
            {
                return this.parkAddress;
            }

            public float GetParkReview()
            {
                return this.parkReview;
            }

            public string GetParkDescription()
            {
                return this.parkDescription;
            }

            public uint GetNumberOfReviews()
            {
                return this.numberOfReviews;
            }

            public void SetParkName(string parkName)
            {
                this.parkName = parkName;
            }

            public void SetParkAddress(string parkAddress)
            {
                this.parkAddress = parkAddress;
            }

            public void SetParkReview(float parkReview)
            {
                this.parkReview = parkReview;
            }

            public void SetParkDescription(string parkDescription)
            {
                this.parkDescription = parkDescription;
            }

            public void SetNumberOfReviews(uint numberOfReviews)
            {
                this.numberOfReviews = numberOfReviews;
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
        public static ParkData[] ReadAllParkDataFromFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            int linesPerPark = 5;
            ParkDataManager.ParkData[] parks = new ParkDataManager.ParkData[lines.Length / linesPerPark];

            for (int i = 0; i < parks.Length; i++)
            {
                int index = i * linesPerPark;
                // this setup is based on the parkdata.txt file
                parks[i] = new ParkDataManager.ParkData
                {
                    parkName = lines[index],
                    parkAddress = lines[index + 1],
                    parkReview = float.Parse(lines[index + 2]),
                };
            }
            return parks;
        }
    }
}
