using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BT_InfluxDB.GameMasks;
using Newtonsoft.Json.Linq;

namespace BT_InfluxDB.API
{
    internal static class Converter
    {
        public class GameplayModifiersConverter : JsonConverter<GameplayModifiers>
        {
            public override void WriteJson(JsonWriter writer, GameplayModifiers value, JsonSerializer serializer)
            {
                //writer.WriteValue(value.ToString());
                throw new NotImplementedException();
            }

            public override GameplayModifiers ReadJson(JsonReader reader, Type objectType, GameplayModifiers existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                //Console.WriteLine(reader.Value);
                JToken token = JToken.Load(reader);

                if (token.Type == JTokenType.Integer)
                {
//#if DEBUG
//                    Console.WriteLine(token.ToObject<int>());
//#endif
                    return GameplayModifiers.CreateFromSerializedData(token.ToObject<int>());
                }
                if (token.Type == JTokenType.String)
                {
//#if DEBUG
//                    Console.WriteLine(int.Parse(token.ToString()));
//#endif
                    return GameplayModifiers.CreateFromSerializedData(int.Parse(token.ToString()));
                }
                return null;

                //GameplayModifierMask mask = (GameplayModifierMask)reader.Value;
                //return new GameplayModifiers.ToModifiers(mask);
            }
        }

        public class BeatmapDifficultyConverter : JsonConverter<BeatmapDifficulty>
        {
            public override void WriteJson(JsonWriter writer, BeatmapDifficulty value, JsonSerializer serializer)
            {
                //writer.WriteValue(value.ToString());
                throw new NotImplementedException();
            }

            public override BeatmapDifficulty ReadJson(JsonReader reader, Type objectType, BeatmapDifficulty existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                JToken token = JToken.Load(reader);

                if (token.Type == JTokenType.Integer)
                {
                    return BeatmapDifficultyMaskExtensions.FromMask(token.ToObject<BeatmapDifficultyMask>());
                }
                if (token.Type == JTokenType.String)
                {
                    return BeatmapDifficultyMaskExtensions.FromMask((BeatmapDifficultyMask)int.Parse(token.ToString()));
                }
                return BeatmapDifficulty.Hard;

                //GameplayModifierMask mask = (GameplayModifierMask)reader.Value;
                //return new GameplayModifiers.ToModifiers(mask);
            }
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new GameplayModifiersConverter(),
                new BeatmapDifficultyConverter(),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}