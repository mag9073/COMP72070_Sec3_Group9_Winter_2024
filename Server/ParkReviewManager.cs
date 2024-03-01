using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ParkReviewManager
    {
        [ProtoContract]
        public class ParkReviewData
        {
            [ProtoMember(1)]
            private string userName;

            [ProtoMember(2)]
            private float parkRating;

            [ProtoMember(3)]
            private DateTime dateOfPosting;

            [ProtoMember(4)]
            private string review;

            public string GetUserName()
            {
                return this.userName;
            }

            public float GetParkRating()
            {
                return this.parkRating;
            }

            public DateTime GetDateOfPosting()
            {
                return this.dateOfPosting;
            }

            public string GetReview()
            {
                return this.review;
            }

            public void SetUserName(string userName)
            {
                this.userName = userName;
            }

            public void SetParkRating(float parkRating)
            {
                this.parkRating = parkRating;
            }

            public void SetDateOfPosting(DateTime dateOfPosting)
            {
                this.dateOfPosting = dateOfPosting;
            }

            public void SetReview(string review)
            {
                this.review = review;
            }

            public byte[] SerializeToByteArray()
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, this);
                    return stream.ToArray();
                }
            }

            public ParkReviewData deserializeLoginData(byte[] buffer)
            {
                using (MemoryStream memStream = new MemoryStream(buffer))
                {
                    return Serializer.Deserialize<ParkReviewData>(memStream);
                }
            }
        }
    }
}
