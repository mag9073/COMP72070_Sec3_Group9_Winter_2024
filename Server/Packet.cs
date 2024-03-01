using System;
using System.Runtime.Serialization;
using System.Text;
using ProtoBuf;

namespace Server
{
    public enum Types
    {
        login, register, send, recv, log
    }

    /********** Head of the Packet **********/
    [ProtoContract]
    public struct Header
    {
        [ProtoMember(1)]
        public byte sourceID;
        [ProtoMember(2)]
        public byte destinationID;
        [ProtoMember(3)]
        public uint sequenceNumber;
        [ProtoMember(4)]
        public uint bodyLength;
        [ProtoMember(5)]
        public Types type;

        public void SetHeaderSourceID(byte sourceID)
        {
            this.sourceID = sourceID;
        }

        public void SetHeaderDestinationID(byte destinationID)
        {
            this.destinationID = destinationID;
        }

        public void SetHeaderSequenceNumber(uint sequenceNumber)
        {
            this.sequenceNumber = sequenceNumber;
        }

        public void SetHeaderBodyLength(uint bodyLength)
        {
            this.bodyLength = bodyLength;
        }

        public byte GetHeaderSourceID()
        {
            return this.sourceID;
        }

        public byte GetHeaderDestinationID()
        {
            return this.destinationID;
        }

        public uint GetHeaderSequenceNumber()
        {
            return this.sequenceNumber;
        }

        public uint GetHeaderBodyLength()
        {
            return this.bodyLength;
        }

        public new Types GetType()
        {
            return this.type;
        }

        public void SetType(Types type)
        {
            this.type = type;
        }
    }

    /********** Body of the Packet **********/
    [ProtoContract]
    public struct Body
    {
        [ProtoMember(1)]
        public byte[] buffer;

        public void SetBodyBuffer(byte[] buffer)
        {
            this.buffer = buffer;
        }

        public byte[] GetBodyBuffer()
        {
            return this.buffer;
        }
    }

    /********** Tail of the Packet **********/
    [ProtoContract]
    public struct Tail
    {
        [ProtoMember(1)]
        public byte[] CRC;

        public void SetTailCRC(byte[] crc)
        {
            this.CRC = crc;
        }

        public byte[] GetTailCRC()
        {
            return this.CRC;
        }
    }

    /************ Packet Definition - Head | Body | Tail - ************/
    [ProtoContract]
    public class Packet
    {
        [ProtoMember(1)]
        private Header header;
        [ProtoMember(2)]
        private Body body;
        [ProtoMember(3)]
        private Tail tail;

        // Default Constructor
        public Packet()
        {
            this.header = new Header();
            this.body = new Body();
            this.tail = new Tail();
        }

        public Packet(byte[] data)
        {
            try
            {
                using (var stream = new MemoryStream(data))
                {
                    Packet packet = Serializer.Deserialize<Packet>(stream);
                    this.header = packet.header;
                    this.body = packet.body;
                    this.tail = packet.tail;
                }
            }
            catch
            {
                // Log error
                throw;
            }
        }


        // Set Head to the Packet
        public void SetPacketHead(byte sourceID, byte destinationID, Types type)
        {
            this.header.SetHeaderSourceID(sourceID);
            this.header.SetHeaderDestinationID(destinationID);
            this.header.SetType(type);
        }


        // Set Body to the Packet
        public void SetPacketBody(byte[] bufferData, uint bodyLength)
        {
            this.header.SetHeaderBodyLength(bodyLength);
            this.body.buffer = new byte[bodyLength];
            this.body.SetBodyBuffer(bufferData);
        }

        // Set Tail to the Packet


        public Header GetPacketHeader()
        {
            return this.header;
        }

        public Body GetBody()
        {
            return this.body;
        }

        public Tail GetTail()
        {
            return this.tail;
        }

        public byte[] getTailBuffer()
        {
            return this.tail.GetTailCRC();

        }

        public byte[] SerializeToByteArray()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, this);
                return stream.ToArray();
            }
        }

    }

}
