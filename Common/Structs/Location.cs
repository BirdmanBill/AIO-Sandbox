﻿using System;
using Common.Interfaces;

namespace Common.Structs
{
    [Serializable]
    public class Location : ICloneable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float O { get; set; }
        public uint Map { get; set; }
        public string Description { get; set; }

        public Location() { }

        public Location(float x, float y, float z, float o, uint map)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.O = o;
            this.Map = map;
        }

        public Location(float x, float y, float z, float o, uint map, string description)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.O = o;
            this.Map = map;
            this.Description = description;
        }


        public void Update(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Update(float x, float y, float z, float o)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.O = o;
        }

        public void Update(IPacketReader packet, bool orientation = false)
        {
            this.X = packet.ReadFloat();
            this.Y = packet.ReadFloat();
            this.Z = packet.ReadFloat();

            if (orientation)
                this.O = packet.ReadFloat();
        }


        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}, O: {O}, Map: {Map}";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
