using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#pragma warning disable CS8618
#pragma warning disable CS8603
namespace BT_InfluxDB.API
{
    public partial class Node
    {
        [JsonProperty("Endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty("Online")]
        public bool Online { get; set; }

        [JsonProperty("StartTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("LastOnline")]
        public DateTimeOffset LastOnline { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("CurrentPlayers")]
        public long CurrentPlayers { get; set; }

        [JsonProperty("CurrentServers")]
        public long CurrentServers { get; set; }
    }

    public partial class Node
    {
        public static List<Node> FromJson(string json) => JsonConvert.DeserializeObject<List<Node>>(json, Converter.Settings);
    }
}
#pragma warning restore CS8618
#pragma warning restore CS8603