using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BT_InfluxDB.API
{
    public static class Serialize
    {
        public static string ToJson(this Lobby self) => JsonConvert.SerializeObject(self, BT_InfluxDB.API.Converter.Settings);

        public static string ToJson(this List<Node> self) => JsonConvert.SerializeObject(self, BT_InfluxDB.API.Converter.Settings);
    }
}
#pragma warning restore CS8618
#pragma warning restore CS8603