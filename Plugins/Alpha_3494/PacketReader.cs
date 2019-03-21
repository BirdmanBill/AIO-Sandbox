﻿using Common.Network;

namespace Alpha_3494
{
    public class PacketReader : BasePacketReader
    {
        public PacketReader(byte[] data, bool parse = true) : base(data)
        {
            if (parse)
            {
                ushort size = ReadUInt16();
                Size = (ushort)((size >> 8) + ((size & 0xFF) << 8) + 2);
                Opcode = ReadUInt32();
            }
        }
    }
}
