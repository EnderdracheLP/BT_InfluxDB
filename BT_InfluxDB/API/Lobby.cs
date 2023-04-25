using System;
using System.Collections.Generic;

using System.Globalization;
using BT_InfluxDB.GameMasks;
using InfluxDB.Client.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#pragma warning disable CS8618
#pragma warning disable CS8603
namespace BT_InfluxDB.API

{
    [Measurement("Lobby")]
    public partial class Lobby
    {
        [Column("Name")]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [Column("Id")]
        [JsonProperty("Id")]
        public string Id { get; set; }

        [Column("EndPoint", IsTag = true)]
        [JsonProperty("EndPoint")]
        public string EndPoint { get; set; }

        [Column("Secret")]
        [JsonProperty("Secret")]
        public string Secret { get; set; }

        [Column("Code")]
        [JsonProperty("Code")]
        public string Code { get; set; }

        [Column("Public")]
        [JsonProperty("Public")]
        public bool Public { get; set; }

        [Column("InGameplay")]
        [JsonProperty("InGameplay")]
        public bool InGameplay { get; set; }

        [Column("beatmap_level_selection_mask")]
        [JsonProperty("beatmap_level_selection_mask")]
        public BeatmapLevelSelectionMask BeatmapLevelSelectionMask { get; set; }

        [Column("gameplay_server_configuration")]
        [JsonProperty("gameplay_server_configuration")]
        public GameplayServerConfiguration GameplayServerConfiguration { get; set; }

        [Column("Player_count")]
        [JsonProperty("Player_count")]
        public long PlayerCount { get; set; }
        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
    }

    public partial class BeatmapLevelSelectionMask
    {
        [Column("difficulties")]
        [JsonProperty("difficulties")]
        public BeatmapDifficulty Difficulties { get; set; }

        [JsonProperty("modifiers")]
        public GameplayModifiers Modifiers { get; set; }

        [JsonProperty("song_packs")]
        public string SongPacks { get; set; }
        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
    }

    public enum InvitePolicy
    {
        OnlyConnectionOwnerCanInvite,
        AnyoneCanInvite,
        NobodyCanInvite
    }

    public enum DiscoveryPolicy : byte
    {
        Hidden,
        WithCode,
        Public
    }

    public enum GameplayServerMode
    {
        Countdown,
        Managed,
        QuickStartOneSong
    }

    public enum SongSelectionMode
    {
        Vote,
        Random,
        OwnerPicks,
        RandomPlayerPicks
    }

    public enum GameplayServerControlSettings
    {
        None = 0,
        AllowModifierSelection = 1,
        AllowSpectate = 2,
        All = 3
    }

    [Measurement("GameplayServerConfiguration")]
    public partial class GameplayServerConfiguration
    {
        [Column("max_player_count")]
        [JsonProperty("max_player_count")]
        public long MaxPlayerCount { get; set; }

        [Column("discovery_policy", IsTag = true)]
        [JsonProperty("discovery_policy")]
        public DiscoveryPolicy DiscoveryPolicy { get; set; }

        [Column("invite_policy", IsTag = true)]
        [JsonProperty("invite_policy")]
        public InvitePolicy InvitePolicy { get; set; }

        [Column("gameplay_server_mode", IsTag = true)]
        [JsonProperty("gameplay_server_mode")]
        public GameplayServerMode GameplayServerMode { get; set; }

        [Column("song_selection_mode", IsTag = true)]
        [JsonProperty("song_selection_mode")]
        public SongSelectionMode SongSelectionMode { get; set; }

        [Column("gameplay_server_control_settings", IsTag = true)]
        [JsonProperty("gameplay_server_control_settings")]
        public GameplayServerControlSettings GameplayServerControlSettings { get; set; }

        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
    }

    public partial class Lobby
    {
        public static Lobby FromJson(string json) => JsonConvert.DeserializeObject<Lobby>(json, BT_InfluxDB.API.Converter.Settings);
    }
}
#pragma warning restore CS8618
#pragma warning restore CS8603