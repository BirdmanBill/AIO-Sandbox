﻿namespace Common.Interfaces
{
    public interface IPacketReader
    {
        uint Opcode { get; set; }
        uint Size { get; set; }
        long Position { get; set; }

        byte ReadByte();

        sbyte ReadInt8();

        short ReadInt16();

        int ReadInt32();

        long ReadInt64();

        byte ReadUInt8();

        ushort ReadUInt16();

        uint ReadUInt32();

        ulong ReadUInt64();

        float ReadFloat();

        double ReadDouble();

        string ReadString(byte terminator = 0);

        string ReadString();

        byte[] ReadBytes(int count);

        byte[] ReadToEnd();

        string ReadStringFromBytes(int count);

        void SkipBytes(int count);
    }
}
