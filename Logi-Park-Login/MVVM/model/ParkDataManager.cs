using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogiPark.MVVM.Model
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
            public float parkReview = float.MinValue;

            [ProtoMember(4)]
            public string parkDescription = String.Empty;

            [ProtoMember(5)]
            public string parkHours = String.Empty;

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

            public void SetParkReview(float parkReview)
            {
                this.parkReview = parkReview;
            }

            public void SetParkDescription(string parkDescription)
            {
                this.parkDescription = parkDescription;
            }

            public void SetParkHours(string parkHours)
            {
                this.parkHours = parkHours;
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

    }
}
