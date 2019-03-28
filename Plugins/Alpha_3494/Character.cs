﻿using Common.Constants;
using Common.Extensions;
using Common.Interfaces;
using Common.Structs;

namespace Alpha_3494
{
    public class Character : BaseCharacter
    {
        public override int Build { get; set; } = Sandbox.Instance.Build;

        public override IPacketWriter BuildUpdate()
        {
            MaskSize = ((int)Fields.MAX + 31) / 32;
            FieldData.Clear();
            MaskArray = new byte[MaskSize * 4];

            PacketWriter writer = new PacketWriter(Sandbox.Instance.Opcodes[global::Opcodes.SMSG_UPDATE_OBJECT], "SMSG_UPDATE_OBJECT");
            writer.WriteUInt32(1); // Number of transactions
            writer.WriteUInt8(2); // UpdateType
            writer.WriteUInt64(Guid); // ObjectGuid
            writer.WriteUInt8(4); // ObjectType, 4 = Player

            writer.WriteUInt32(0);  // MovementFlagMask
            writer.WriteFloat(Location.X);  // x
            writer.WriteFloat(Location.Y);  // y
            writer.WriteFloat(Location.Z);  // z
            writer.WriteFloat(Location.O);  // w (o)

            writer.WriteFloat(2.5f); // WalkSpeed
            writer.WriteFloat(7.0f); // RunSpeed
            writer.WriteFloat(4.7222f); // SwimSpeed
            writer.WriteFloat(3.14f); // TurnSpeed

            writer.WriteUInt32(1); // Flags, 1 - Player
            writer.WriteUInt32(1); // AttackCycle
            writer.WriteUInt32(0); // TimerId
            writer.WriteUInt64(0); // VictimGuid

            SetField(Fields.GUID, Guid);
            SetField(Fields.HIER_TYPE, (uint)0x19);
            SetField(Fields.ENTRY, 0);
            SetField(Fields.SCALE, Scale);
            SetField(Fields.TARGET, (ulong)0);
            SetField(Fields.HEALTH, Health);
            SetField(Fields.MANA, Mana);
            SetField(Fields.RAGE, 0);
            SetField(Fields.FOCUS, Focus);
            SetField(Fields.ENERGY, Energy);
            SetField(Fields.MAX_HEALTH, Health);
            SetField(Fields.MAX_MANA, Mana);
            SetField(Fields.MAX_RAGE, Rage);
            SetField(Fields.MAX_FOCUS, Focus);
            SetField(Fields.MAX_ENERGY, Energy);
            SetField(Fields.LEVEL, Level);
            SetField(Fields.FACTION, 35);
            SetField(Fields.UNIT_BYTES_0, ToUInt32(Race, Class, Gender, PowerType));
            SetField(Fields.STRENGTH, Strength);
            SetField(Fields.AGILITY, Agility);
            SetField(Fields.STAMINA, Stamina);
            SetField(Fields.INTELLECT, Intellect);
            SetField(Fields.SPIRIT, Spirit);
            SetField(Fields.BASE_STRENGTH, Strength);
            SetField(Fields.BASE_AGILITY, Agility);
            SetField(Fields.BASE_STAMINA, Stamina);
            SetField(Fields.BASE_INTELLECT, Intellect);
            SetField(Fields.BASE_SPIRIT, Spirit);
            SetField(Fields.FLAGS, 0);
            SetField(Fields.DISPLAYID, DisplayId);
            SetField(Fields.MOUNT_DISPLAYID, MountDisplayId);
            SetField(Fields.UNIT_BYTES_1, ToUInt32((byte)StandState));
            SetField(Fields.PLAYER_SELECTION, (ulong)0);
            SetField(Fields.PLAYER_BYTES_1, ToUInt32(Skin, Face, HairStyle, HairColor));
            SetField(Fields.PLAYER_BYTES_2, ToUInt32(b2: FacialHair, b4: RestedState));
            SetField(Fields.PLAYER_XP, 47);
            SetField(Fields.PLAYER_NEXTLEVEL_XP, 200);
            SetField(Fields.DAMAGE, 0); // max_dmg << 16 | min_dmg
            SetField(Fields.BASEATTACKTIME0, 1);

            for (int i = 0; i < 32; i++)
                SetField(Fields.EXPLORED_ZONES + i, 0xFFFFFFFF);

            // FillInPartialObjectData
            writer.WriteUInt8(MaskSize); // UpdateMaskBlocks
            writer.Write(MaskArray);
            foreach (var kvp in FieldData)
                writer.Write(kvp.Value); // Data

            return writer;
        }

        public override IPacketWriter BuildMessage(string text)
        {
            PacketWriter message = new PacketWriter(Sandbox.Instance.Opcodes[global::Opcodes.SMSG_MESSAGECHAT], "SMSG_MESSAGECHAT");
            return this.BuildMessage(message, text);
        }

        public override void Teleport(float x, float y, float z, float o, uint map, ref IWorldManager manager)
        {
            IsTeleporting = true;

            if (Location.Map == map)
            {
                PacketWriter movementStatus = new PacketWriter(Sandbox.Instance.Opcodes[global::Opcodes.SMSG_MOVE_WORLDPORT_ACK], "SMSG_MOVE_WORLDPORT_ACK");
                movementStatus.WriteUInt32(0); // Transport ID
                movementStatus.WriteFloat(x);
                movementStatus.WriteFloat(y);
                movementStatus.WriteFloat(z);
                movementStatus.WriteFloat(o);
                movementStatus.WriteFloat(0);
                movementStatus.WriteUInt32(0); // Movement Flags
                manager.Send(movementStatus);

                System.Threading.Thread.Sleep(150);
            }
            else
            {
                // Loading screen
                PacketWriter transferPending = new PacketWriter(Sandbox.Instance.Opcodes[global::Opcodes.SMSG_TRANSFER_PENDING], "SMSG_TRANSFER_PENDING");
                transferPending.WriteUInt32(map);
                manager.Send(transferPending);

                System.Threading.Thread.Sleep(300);

                // New world transfer
                PacketWriter newWorld = new PacketWriter(Sandbox.Instance.Opcodes[global::Opcodes.SMSG_NEW_WORLD], "SMSG_NEW_WORLD");
                newWorld.WriteUInt8((byte)map);
                newWorld.WriteFloat(x);
                newWorld.WriteFloat(y);
                newWorld.WriteFloat(z);
                newWorld.WriteFloat(o);
                manager.Send(newWorld);

                // HACK something is missing here
                // if this delay is too quick the client throws an exception trying to calculate fall damage
                System.Threading.Thread.Sleep(3000);
            }

            Location = new Location(x, y, z, o, map);
            manager.Send(BuildUpdate());

            IsTeleporting = false;
        }

        public override IPacketWriter BuildForceSpeed(float modifier, SpeedType type = SpeedType.Run)
        {
            var opcode = type == SpeedType.Swim ? global::Opcodes.SMSG_FORCE_SWIM_SPEED_CHANGE : global::Opcodes.SMSG_FORCE_SPEED_CHANGE;
            PacketWriter writer = new PacketWriter(Sandbox.Instance.Opcodes[opcode], opcode.ToString());
            return this.BuildForceSpeed(writer, modifier);
        }

        internal enum Fields
        {
            GUID = 0,
            HIER_TYPE = 2,
            ENTRY = 3,
            SCALE = 4,
            PADDING = 5,
            CHARMED = 6,
            SUMMON = 8,
            CHARMEDBY = 10,
            SUMMONEDBY = 12,
            CREATEDBY = 14,
            TARGET = 16,
            COMBO_TARGET = 18,
            CHANNEL_OBJECT = 20,
            HEALTH = 22,
            POWER0 = 23,
            MANA = 23,
            RAGE = 24,
            FOCUS = 25,
            ENERGY = 26,
            MAX_HEALTH = 27,
            MAX_POWER0 = 28,
            MAX_MANA = 28,
            MAX_RAGE = 29,
            MAX_FOCUS = 30,
            MAX_ENERGY = 31,
            LEVEL = 32,
            FACTION = 33,
            UNIT_BYTES_0 = 34,
            STATS0 = 35,
            STRENGTH = 35,
            AGILITY = 36,
            STAMINA = 37,
            INTELLECT = 38,
            SPIRIT = 39,
            BASE_STRENGTH = 40,
            BASE_AGILITY = 41,
            BASE_STAMINA = 42,
            BASE_INTELLECT = 43,
            BASE_SPIRIT = 44,
            VIRTUAL_ITEMSLOTDISPLAY = 45,
            VIRTUAL_ITEMINFO = 48,
            FLAGS = 54,
            COINAGE = 55,
            AURA = 56,
            AURA_LEVELS = 112,
            AURA_APPLICATIONS = 122,
            AURA_FLAGS = 132,
            AURA_STATE = 139,
            BASEATTACKTIME0 = 140,
            BASEATTACKTIME1 = 141,
            RESISTANCE = 142,
            RESIST_PHYSICAL = 142,
            RESIST_HOLY = 143,
            RESIST_FIRE = 144,
            RESIST_NATURE = 145,
            RESIST_FROST = 146,
            RESIST_SHADOW = 147,
            BOUNDING_RADIUS = 148,
            COMBAT_REACH = 149,
            WEAPON_REACH = 150,
            DISPLAYID = 151,
            MOUNT_DISPLAYID = 152,
            DAMAGE = 153,
            MOD_DAMAGE_DONE = 154,
            RESISTANCE_BUFF_POSITIVE = 160,
            RESISTANCE_BUFF_NEGATIVE = 166,
            RESISTANCE_ITEM_MODS = 172,
            UNIT_BYTES_1 = 178,
            PET_NUMBER = 179,
            PET_NAME_TIMESTAMP = 180,
            PET_EXPERIENCE = 181,
            PET_NEXT_LEVE_EXP = 182,
            DYNAMIC_FLAGS = 183,
            EMOTE_STATE = 184,
            CHANNEL_SPELL = 185,
            MOD_CAST_SPEED = 186,
            CREATED_BY_SPELL = 187,
            NPC_FLAGS = 188,
            UNIT_BYTES_2 = 189,
            ATTACKPOWER = 190,
            UNIT_PADDING = 191,
            PLAYER_FIELD_INV_SLOT_1 = 192,
            PLAYER_FIELD_PACK_SLOT_1 = 238,
            PLAYER_FIELD_BANK_SLOT_1 = 270,
            PLAYER_FIELD_BANKBAG_SLOT_1 = 318,
            PLAYER_SELECTION = 330,
            PLAYER_FARSIGHT = 332,
            PLAYER_DUEL_ARBITER = 334,
            PLAYER_FIELD_NUM_INV_SLOTS = 336,
            PLAYER_GUILDID = 329,
            PLAYER_GUILDRANK = 338,
            PLAYER_BYTES_1 = 339,
            PLAYER_XP = 340,
            PLAYER_NEXTLEVEL_XP = 341,
            PLAYER_SKILL_INFO_1_1 = 342,
            PLAYER_BYTES_2 = 534,
            PLAYER_QUEST_LOG_1_1 = 545,
            PLAYER_CHARACTER_POINTS1 = 615,
            PLAYER_CHARACTER_POINTS2 = 616,
            PLAYER_TRACK_CREATURES = 617,
            PLAYER_TRACK_RESOURCES = 618,
            PLAYER_CHAT_FILTERS = 619,
            PLAYER_DUEL_TEAM = 620,
            PLAYER_BLOCK_PERCENTAGE = 621,
            PLAYER_DODGE_PERCENTAGE = 622,
            PLAYER_PARRY_PERCENTAGE = 623,
            PLAYER_BASE_MANA = 624,
            PLAYER_GUILD_TIMESTAMP = 625,
            EXPLORED_ZONES = 626,
            MAX = 658,
        }
    }
}
