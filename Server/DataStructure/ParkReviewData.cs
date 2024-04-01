using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataStructure
{
    [ProtoContract]
    public class ParkReviewData
    {
        [ProtoMember(1)]
        public string ParkName = String.Empty;

        [ProtoMember(2)]
        public string UserName = String.Empty;

        [ProtoMember(3)]
        public float Rating = float.MinValue;

        [ProtoMember(4)]
        public DateTime DateOfPosting = DateTime.MinValue;

        [ProtoMember(5)]
        public string Review = String.Empty;

        public string GetUserName()
        {
            return this.UserName;
        }

        public float GetParkRating()
        {
            return this.Rating;
        }

        public DateTime GetDateOfPosting()
        {
            return this.DateOfPosting;
        }

        public string GetReview()
        {
            return this.Review;
        }

        public void SetUserName(string userName)
        {
            this.UserName = userName;
        }

        public void SetParkRating(float parkRating)
        {
            this.Rating = parkRating;
        }

        public void SetDateOfPosting(DateTime dateOfPosting)
        {
            this.DateOfPosting = dateOfPosting;
        }

        public void SetReview(string review)
        {
            this.Review = review;
        }

        public byte[] SerializeToByteArray()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, this);
                return stream.ToArray();
            }
        }

        public ParkReviewData deserializeParkReviewData(byte[] buffer)
        {
            using (MemoryStream memStream = new MemoryStream(buffer))
            {
                memStream.Position = 0;
                return Serializer.Deserialize<ParkReviewData>(memStream);
            }
        }

        public void deserializeParkReviewData(byte[] data, int offset, int size)
        {
            using (MemoryStream ms = new MemoryStream(data, offset, size))
            {
                ParkReviewData deserializedData = Serializer.Deserialize<ParkReviewData>(ms);
                this.ParkName = deserializedData.ParkName;
                this.UserName = deserializedData.UserName;
                this.DateOfPosting = deserializedData.DateOfPosting;
                this.Rating = deserializedData.Rating;
                this.Review = deserializedData.Review;
            }
        }

    }
}
