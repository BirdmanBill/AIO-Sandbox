﻿using Common.Interfaces;
using Common.Interfaces.Handlers;
using Vanilla_5428.Handlers;

namespace Vanilla_5428
{
    public class Sandbox : ISandbox
    {
        public static Sandbox Instance { get; } = new Sandbox();

        public string RealmName { get; set; } = "Vanilla (1.11.X) Sandbox";
        public int Expansion => 1;
        public int Build { get; set; } = 5428;
        public int RealmPort { get; set; } = 3724;
        public int RedirectPort { get; set; } = 9002;
        public int WorldPort { get; set; } = 8129;

        public IOpcodes Opcodes { get; set; } = new Opcodes();

        public IAuthHandler AuthHandler { get; set; } = new AuthHandler();
        public ICharHandler CharHandler { get; set; } = new CharHandler();
        public IWorldHandler WorldHandler { get; set; } = new WorldHandler();

        public IPacketReader ReadPacket(byte[] data, bool parse = true) => new PacketReader(data, parse);

        public IPacketWriter WritePacket() => new PacketWriter();
    }
}
