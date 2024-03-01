using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MVVM.Model
{
    public class ParkDataManager
    {
        [ProtoContract]
        public class ParkData
        {
            [ProtoMember(1)]
            public string parkName;

            [ProtoMember(2)]
            public float parkReview;

            [ProtoMember(3)]
            public string parkDescription;

            [ProtoMember(4)]
            public uint numberOfReviews;

            public string GetParkName()
            {
                return this.parkName;
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

            public ParkData deserializeLoginData(byte[] buffer)
            {
                using (MemoryStream memStream = new MemoryStream(buffer))
                {
                    return Serializer.Deserialize<ParkData>(memStream);
                }
            }
        }
    }
}
