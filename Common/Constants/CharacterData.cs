﻿using System.Collections.Generic;

namespace Common.Constants
{
    public static class CharacterData
    {
        public const uint COMMON_SKILL_ID = 98;
        public const uint COMMON_SPELL_ID = 668;
        public const uint ORCISH_SKILL_ID = 109;
        public const uint ORCISH_SPELL_ID = 669;

        public static readonly Dictionary<Races, uint[]> DisplayIds = new Dictionary<Races, uint[]>()
        {
            { Races.HUMAN,     new []{ 0x31u, 0x32u } },
            { Races.ORC,       new []{ 0x33u, 0x34u } },
            { Races.DWARF,     new []{ 0x35u, 0x36u } },
            { Races.NIGHT_ELF, new []{ 0x37u, 0x38u } },
            { Races.UNDEAD,    new []{ 0x39u, 0x3Au } },
            { Races.TAUREN,    new []{ 0x3Bu, 0x3Cu } },
            { Races.GNOME,     new []{ 0x61Bu, 0x61Cu } },
            { Races.TROLL,     new []{ 0x5C6u, 0x5C7u } },
            { Races.BLOODELF,  new []{ 0x3C74u, 0x3C73u } },
            { Races.DRAENEI,   new []{ 0x3EFDu, 0x3EFEu } },
        };

        public static readonly Dictionary<Races, uint> FactionTemplates = new Dictionary<Races, uint>()
        {
            { Races.HUMAN,     (uint)Races.HUMAN },
            { Races.ORC,       (uint)Races.ORC },
            { Races.DWARF,     (uint)Races.DWARF },
            { Races.NIGHT_ELF, (uint)Races.NIGHT_ELF },
            { Races.UNDEAD,    (uint)Races.UNDEAD },
            { Races.TAUREN,    (uint)Races.TAUREN },
            { Races.GNOME,     (uint)Races.GNOME },
            { Races.TROLL,     116 },
            { Races.BLOODELF,  1610 },
            { Races.DRAENEI,   1629 },
        };
    }
}
