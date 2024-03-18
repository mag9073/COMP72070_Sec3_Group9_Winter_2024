using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataStructure
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

        public ParkData deserializeParkData(byte[] buffer)
        {
            using (MemoryStream memStream = new MemoryStream(buffer))
            {
                return Serializer.Deserialize<ParkData>(memStream);
            }
        }

        public void deserializeParkData(byte[] data, int offset, int size)
        {
            using (MemoryStream ms = new MemoryStream(data, offset, size))
            {
                ParkData deserializedData = Serializer.Deserialize<ParkData>(ms);
                this.parkName = deserializedData.parkName;
                this.parkAddress = deserializedData.parkAddress;
                this.parkDescription = deserializedData.parkDescription;
                this.parkHours = deserializedData.parkHours;
            }
        }

    }
}
