using BT_InfluxDB.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT_InfluxDB.GameMasks
{
    public enum BeatmapDifficulty
    {
        Easy,
        Normal,
        Hard,
        Expert,
        ExpertPlus
    }
    public static class BeatmapDifficultyMaskExtensions
    {
        public static BeatmapDifficulty FromMask(this BeatmapDifficultyMask mask)
        {
            switch (mask)
            {
                case BeatmapDifficultyMask.Easy:
                    return BeatmapDifficulty.Easy;
                case BeatmapDifficultyMask.Normal:
                    return BeatmapDifficulty.Normal;
                case BeatmapDifficultyMask.Easy | BeatmapDifficultyMask.Normal:
                    break;
                case BeatmapDifficultyMask.Hard:
                    return BeatmapDifficulty.Hard;
                default:
                    if (mask == BeatmapDifficultyMask.Expert)
                    {
                        return BeatmapDifficulty.Expert;
                    }
                    if (mask == BeatmapDifficultyMask.ExpertPlus)
                    {
                        return BeatmapDifficulty.ExpertPlus;
                    }
                    break;
            }
            return BeatmapDifficulty.Hard;
        }
    }
}
