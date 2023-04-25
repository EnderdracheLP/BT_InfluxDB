using BT_InfluxDB.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BT_InfluxDB.GameMasks
{
    public class GameplayModifiers
    {
        public GameplayModifiers.EnergyType energyType
        {
            get
            {
                return this._energyType;
            }
        }

        public bool noFailOn0Energy
        {
            get
            {
                return this._noFailOn0Energy;
            }
        }

        public bool instaFail
        {
            get
            {
                return this._instaFail;
            }
        }

        public bool failOnSaberClash
        {
            get
            {
                return this._failOnSaberClash;
            }
        }

        public GameplayModifiers.EnabledObstacleType enabledObstacleType
        {
            get
            {
                return this._enabledObstacleType;
            }
        }

        public bool fastNotes
        {
            get
            {
                return this._fastNotes;
            }
        }

        public bool strictAngles
        {
            get
            {
                return this._strictAngles;
            }
        }

        public bool disappearingArrows
        {
            get
            {
                return this._disappearingArrows;
            }
        }

        public bool ghostNotes
        {
            get
            {
                return this._ghostNotes;
            }
        }

        public bool noBombs
        {
            get
            {
                return this._noBombs;
            }
        }

        public GameplayModifiers.SongSpeed songSpeed
        {
            get
            {
                return this._songSpeed;
            }
        }

        public bool noArrows
        {
            get
            {
                return this._noArrows;
            }
        }

        public bool proMode
        {
            get
            {
                return this._proMode;
            }
        }

        public bool zenMode
        {
            get
            {
                return this._zenMode;
            }
        }

        public bool smallCubes
        {
            get
            {
                return this._smallCubes;
            }
        }

        protected GameplayModifiers.EnergyType _energyType;

        protected bool _noFailOn0Energy;

        protected bool _instaFail;

        protected bool _failOnSaberClash;

        protected GameplayModifiers.EnabledObstacleType _enabledObstacleType;

        protected bool _fastNotes;

        protected bool _strictAngles;

        protected bool _disappearingArrows;

        protected bool _ghostNotes;

        protected bool _noBombs;

        protected GameplayModifiers.SongSpeed _songSpeed;

        protected bool _noArrows;

        protected bool _proMode;

        protected bool _zenMode;

        protected bool _smallCubes;

        public GameplayModifiers(GameplayModifiers.EnergyType energyType, bool noFailOn0Energy, bool instaFail, bool failOnSaberClash, GameplayModifiers.EnabledObstacleType enabledObstacleType, bool noBombs, bool fastNotes, bool strictAngles, bool disappearingArrows, GameplayModifiers.SongSpeed songSpeed, bool noArrows, bool ghostNotes, bool proMode, bool zenMode, bool smallCubes)
        {
            this._energyType = energyType;
            this._noFailOn0Energy = noFailOn0Energy;
            this._instaFail = instaFail;
            this._failOnSaberClash = failOnSaberClash;
            this._enabledObstacleType = enabledObstacleType;
            this._noBombs = noBombs;
            this._fastNotes = fastNotes;
            this._strictAngles = strictAngles;
            this._disappearingArrows = disappearingArrows;
            this._songSpeed = songSpeed;
            this._noArrows = noArrows;
            this._ghostNotes = ghostNotes;
            this._proMode = proMode;
            this._zenMode = zenMode;
            this._smallCubes = smallCubes;
        }

        public static GameplayModifiers ToModifiers(GameplayModifierMask gameplayModifierMask)
        {
            return new GameplayModifiers(((gameplayModifierMask & GameplayModifierMask.BatteryEnergy) != GameplayModifierMask.None) ? GameplayModifiers.EnergyType.Battery : GameplayModifiers.EnergyType.Bar, (gameplayModifierMask & GameplayModifierMask.NoFail) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.InstaFail) > GameplayModifierMask.None, false, ((gameplayModifierMask & GameplayModifierMask.NoObstacles) != GameplayModifierMask.None) ? GameplayModifiers.EnabledObstacleType.NoObstacles : GameplayModifiers.EnabledObstacleType.All, (gameplayModifierMask & GameplayModifierMask.NoBombs) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.FastNotes) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.StrictAngles) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.DisappearingArrows) > GameplayModifierMask.None, ((gameplayModifierMask & GameplayModifierMask.FasterSong) != GameplayModifierMask.None) ? GameplayModifiers.SongSpeed.Faster : (((gameplayModifierMask & GameplayModifierMask.SlowerSong) != GameplayModifierMask.None) ? GameplayModifiers.SongSpeed.Slower : (((gameplayModifierMask & GameplayModifierMask.SuperFastSong) != GameplayModifierMask.None) ? GameplayModifiers.SongSpeed.SuperFast : GameplayModifiers.SongSpeed.Normal)), (gameplayModifierMask & GameplayModifierMask.NoArrows) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.GhostNotes) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.ProMode) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.ZenMode) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.SmallCubes) > GameplayModifierMask.None);
        }

        public static GameplayModifiers CreateFromSerializedData(int @int)
        {
            GameplayModifiers.EnergyType energyType = (GameplayModifiers.EnergyType)(@int & 15);
            bool flag = (@int & 64) != 0;
            bool flag2 = (@int & 128) != 0;
            GameplayModifiers.EnabledObstacleType enabledObstacleType = (GameplayModifiers.EnabledObstacleType)((@int >> 8) & 15);
            bool flag3 = (@int & 8192) != 0;
            bool flag4 = (@int & 16384) != 0;
            bool flag5 = (@int & 32768) != 0;
            bool flag6 = (@int & 65536) != 0;
            bool flag7 = (@int & 131072) != 0;
            GameplayModifiers.SongSpeed songSpeed = (GameplayModifiers.SongSpeed)((@int >> 18) & 15);
            bool flag8 = (@int & 4194304) != 0;
            return new GameplayModifiers(energyType, (@int & 8388608) != 0, flag, flag2, enabledObstacleType, flag3, flag4, flag5, flag6, songSpeed, flag8, flag7, (@int & 16777216) != 0, (@int & 33554432) != 0, (@int & 67108864) != 0);
        }

        public enum EnabledObstacleType
        {
            All,
            FullHeightOnly,
            NoObstacles
        }

        public enum EnergyType
        {
            Bar,
            Battery
        }

        public enum SongSpeed
        {
            Normal,
            Faster,
            Slower,
            SuperFast
        }
    }
}
