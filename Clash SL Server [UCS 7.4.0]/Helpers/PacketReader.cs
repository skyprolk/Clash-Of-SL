using System;
using System.IO;
using System.Text;

namespace CSS.Helpers
{
    //Obsolate
    internal class PacketReader12121 : BinaryReader
    {
        public PacketReader12121(Stream stream) : base(stream)
        {
        }

        byte[] ReadBytesWithEndian(int count, bool switchEndian = true)
        {
            byte[] buffer = new byte[count];
            BaseStream.Read(buffer, 0, count);
            if (BitConverter.IsLittleEndian && switchEndian)
                Array.Reverse(buffer);
            return buffer;
        }

        public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, 0, count);

        public override bool ReadBoolean()
        {
            byte state = ReadByte();
            switch (state)
            {
                case 1:
                    return true;

                case 0:
                    return false;

                default:
                    return false;
            }
        }

        public override byte ReadByte() => (byte)BaseStream.ReadByte();

        public byte[] ReadByteArray()
        {
            int length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1 || length > BaseStream.Length - BaseStream.Position)
                return null;
            byte[] buffer = ReadBytesWithEndian(length, false);
            return buffer;
        }

        public override short ReadInt16() => (short)ReadUInt16();

        public int ReadInt24()
        {
            byte[] packetLengthBuffer = ReadBytesWithEndian(3, false);
            return (packetLengthBuffer[0] << 16) | (packetLengthBuffer[1] << 8) | packetLengthBuffer[2];
        }

        public override int ReadInt32() => (int)ReadUInt32();

        public override long ReadInt64() => (long)ReadUInt64();

        public override string ReadString()
        {
            int _Length = this.ReadInt32();
            if ((_Length == -1) || (_Length < -1) || (_Length > this.BaseStream.Length - this.BaseStream.Position))
            {
                return null;
            }

            byte[] _Buffer = this.ReadBytesWithEndian(_Length, false);
            return Encoding.UTF8.GetString(_Buffer);
        }

        public override ushort ReadUInt16()
        {
            byte[] buffer = ReadBytesWithEndian(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public uint ReadUInt24() => (uint)ReadInt24();

        public override uint ReadUInt32()
        {
            byte[] buffer = ReadBytesWithEndian(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public override ulong ReadUInt64()
        {
            byte[] buffer = ReadBytesWithEndian(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);
    }
}
