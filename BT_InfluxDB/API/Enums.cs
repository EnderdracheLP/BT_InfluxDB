﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT_InfluxDB.API
{
    public enum GameplayModifierMask : ushort
    {
        None = 0,
        BatteryEnergy = 1,
        NoFail = 2,
        InstaFail = 4,
        NoObstacles = 8,
        NoBombs = 16,
        FastNotes = 32,
        StrictAngles = 64,
        DisappearingArrows = 128,
        FasterSong = 256,
        SlowerSong = 512,
        NoArrows = 1024,
        GhostNotes = 2048,
        SuperFastSong = 4096,
        ProMode = 8192,
        ZenMode = 16384,
        SmallCubes = 32768,
        All = 65535
    }

    public enum BeatmapDifficultyMask : byte
    {
        Easy = 1,
        Normal = 2,
        Hard = 4,
        Expert = 8,
        ExpertPlus = 16,
        All = 31
    }
}
