/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System;
using System.IO;
using System.Text;

namespace CSS.Helpers
{
    /// <summary>
    ///     Implements methods to read Clash of Clans packets.
    /// </summary>
    public class CoCSharpPacketReader : BinaryReader
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PacketReader" /> class with the specified
        ///     base <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The base stream.</param>
        public CoCSharpPacketReader(Stream stream)
            : base(stream)
        {
            // Space
        }

        #endregion Public Constructors

        #region Private Methods

        byte[] ReadBytesWithEndian(int count, bool switchEndian = true)
        {
            var buffer = new byte[count];
            BaseStream.Read(buffer, 0, count);
            if (BitConverter.IsLittleEndian && switchEndian)
                Array.Reverse(buffer);
            return buffer;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        ///     Reads a sequence of bytes from the underlying stream and advances the position within the
        ///     stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">
        ///     The byte array which contains the read bytes from the underlying stream.
        /// </param>
        /// <param name="offset">The zero-based index at which to begin reading data.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>The number of byte read.</returns>
        public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, 0, count);

        /// <summary>
        ///     Reads a <see cref="bool" /> from the underlying stream.
        /// </summary>
        /// <returns><see cref="bool" /> read.</returns>
        public override bool ReadBoolean()
        {
            var state = ReadByte();
            switch (state)
            {
                case 1:
                    return true;

                case 0:
                    return false;

                default:
                    throw new Exception("Invalid.");
            }
        }

        /// <summary>
        ///     Reads a <see cref="byte" /> from the underlying stream.
        /// </summary>
        /// <returns><see cref="byte" /> read.</returns>
        public override byte ReadByte() => (byte)BaseStream.ReadByte();

        /// <summary>
        ///     Reads an array of <see cref="byte" /> from the underlying stream.
        /// </summary>
        /// <returns>The array of <see cref="byte" /> read.</returns>
        public byte[] ReadByteArray()
        {
            var length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1)
                throw new Exception("A byte array length was incorrect: " + length + ".");
            if (length > BaseStream.Length - BaseStream.Position)
                throw new Exception(string.Format("A byte array was larger than remaining bytes. {0} > {1}.", length,
                    BaseStream.Length - BaseStream.Position));
            var buffer = ReadBytesWithEndian(length, false);
            return buffer;
        }

        /// <summary>
        ///     Reads an <see cref="short" /> from the underlying stream.
        /// </summary>
        /// <returns><see cref="short" /> read.</returns>
        public override short ReadInt16() => (short)ReadUInt16();

        /// <summary>
        ///     Reads a 3 bytes long int. Clash of Clans packets uses this to encode there length.
        /// </summary>
        /// <returns>3 bytes int.</returns>
        public int ReadInt24()
        {
            var packetLengthBuffer = ReadBytesWithEndian(3, false);
            return (packetLengthBuffer[0] << 16) | (packetLengthBuffer[1] << 8) | packetLengthBuffer[2];
        }

        /// <summary>
        ///     Reads an <see cref="int" /> from the underlying stream.
        /// </summary>
        /// <returns><see cref="int" /> read.</returns>
        public override int ReadInt32() => (int)ReadUInt32();

        /// <summary>
        ///     Reads an <see cref="long" /> from the underlying stream.
        /// </summary>
        /// <returns><see cref="long" /> read.</returns>
        public override long ReadInt64() => (long)ReadUInt64();

        /// <summary>
        ///     Reads a <see cref="string" /> from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public override string ReadString()
        {
            var length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1)
                throw new Exception("A string length was incorrect: " + length);
            if (length > BaseStream.Length - BaseStream.Position)
                throw new Exception(string.Format("A string was larger than remaining bytes. {0} > {1}.", length,
                    BaseStream.Length - BaseStream.Position));
            var buffer = ReadBytesWithEndian(length, false);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        ///     Reads a <see cref="ushort" /> from the underlying stream.
        /// </summary>
        /// <returns><see cref="ushort" /> read.</returns>
        public override ushort ReadUInt16()
        {
            var buffer = ReadBytesWithEndian(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        ///     Reads a 3 bytes long uint.
        /// </summary>
        /// <returns>3 bytes int.</returns>
        public uint ReadUInt24() => (uint)ReadInt24();

        /// <summary>
        ///     Reads a <see cref="uint" /> from the underlying stream.
        /// </summary>
        /// <returns><see cref="uint" /> read.</returns>
        public override uint ReadUInt32()
        {
            var buffer = ReadBytesWithEndian(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        ///     Reads a <see cref="ulong" /> from the underlying stream.
        /// </summary>
        /// <returns><see cref="ulong" /> read.</returns>
        public override ulong ReadUInt64()
        {
            var buffer = ReadBytesWithEndian(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        ///     Sets the position of the underlying stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">
        ///     A value of type <see cref="SeekOrigin" /> indicating the reference point used to obtain
        ///     the new position.
        /// </param>
        /// <returns>The new position of the underlying stream.</returns>
        public long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);

        #endregion Public Methods
    }
}
