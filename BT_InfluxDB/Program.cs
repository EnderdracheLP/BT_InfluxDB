using System;
using System.Linq;
using System.Threading.Tasks;
using BT_InfluxDB.API;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;
using Newtonsoft.Json;

namespace BT_InfluxDB
{
    public class Client
    {
        public static async Task Main(string[] args)
        {
            // You can generate an API token from the "API Tokens Tab" in the UI
            string? token = Environment.GetEnvironmentVariable("BT_INFLUX_TOKEN");
            string? bucket = Environment.GetEnvironmentVariable("BT_INFLUX_BUCKET");
            string? org = Environment.GetEnvironmentVariable("BT_INFLUX_ORG");
            string? databaseEndpoint = Environment.GetEnvironmentVariable("BT_INFLUX_ENDPOINT");
            string? btAPI = Environment.GetEnvironmentVariable("BT_API");
            if (token == null || bucket == null || org == null || databaseEndpoint == null || btAPI == null)
            {
                // Log an error and exit
                Console.Error.WriteLine("Missing required environment variables. Please set BT_INFLUX_TOKEN, BT_INFLUX_BUCKET, BT_INFLUX_ORG, BT_INFLUX_ENDPOINT and BT_API.");
                Environment.Exit(10);
            }


            //const string token = "AELaMaklPB1yX2bcEdMQH_JzltvghJd3LnqoU-2hC8Da0a1Z1LSjMLe_Byxef1E9Ij3lq9z9o_UwVVglOZOh_A==";
            //const string bucket = "test_bt";
            //const string org = "michael-r-elp";

            //try
            //{
            using var client = new InfluxDBClient(databaseEndpoint, token);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Exception trying to connect to InfluxDB: {ex.Message}");
            //}

            // Example using WriteRecord
            //const string data = "mem,host=host1 used_percent=23.43234543";
            //using (var writeApi = client.GetWriteApi())
            //{
            //    writeApi.WriteRecord(data, WritePrecision.Ns, bucket, org);
            //}

            // Example using WritePoint
            //var point = PointData
            //  .Measurement("mem")
            //  .Tag("host", "host1")
            //  .Field("used_percent", 23.43234543)
            //  .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            //using (var writeApi = client.GetWriteApi())
            //{
            //    writeApi.WritePoint(point, bucket, org);
            //}

            // Get JSON Data from BeatTogether MasterServer API endpoint for processing
            while (true)
            {
                try
                {
                    var jsonNodes = await new System.Net.Http.HttpClient().GetStringAsync($"{btAPI}/get_nodes");
                    var nodes = API.Node.FromJson(jsonNodes);

                    //var timestamp = DateTime.UtcNow;

                    foreach (var node in nodes)
                    {
#if DEBUG
                        Console.WriteLine(node.Endpoint);
                        Console.WriteLine(node.Online);
                        Console.WriteLine(node.StartTime);
                        Console.WriteLine(node.LastOnline);
                        Console.WriteLine(node.Version);
                        Console.WriteLine(node.CurrentPlayers);
                        Console.WriteLine(node.CurrentServers);

#endif
                    // Write to InfluxDB
                    foreach (var node in nodes)
                    {
                        var point = PointData
                            .Measurement("NodeStats")
                            .Tag("endpoint", node.Endpoint)
                            .Tag("version", node.Version)
                            .Field("CurrentPlayers", node.CurrentPlayers)
                            .Field("CurrentServers", node.CurrentServers)
                            .Field("Online", node.Online)
                            .Timestamp(node.LastOnline, WritePrecision.Ns);
                        //var point = PointData
                        //    .Measurement("nodes")
                        //    .Tag("endpoint", node.Endpoint)
                        //    .Field("online", node.Online)
                        //    .Field("starttime", node.StartTime)
                        //    .Field("lastonline", node.LastOnline)
                        //    .Field("version", node.Version)
                        //    .Field("currentplayers", node.CurrentPlayers)
                        //    .Field("currentservers", node.CurrentServers)
                        //    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
                        using (var writeApi = client.GetWriteApi())
                        {
                            writeApi.WritePoint(point, bucket, org);
                        }
                    }

                    List<string>? secrets = null;
                    try
                    {
                        var jsonSecrets = await new HttpClient().GetStringAsync("http://master.beattogether.systems:8989/get_public_server_secrets");
                        secrets = JsonConvert.DeserializeObject<List<string>>(jsonSecrets);
                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            secrets = null;
                        }
                    }

                    if (secrets == null || secrets.Count == 0)
                    {
                        Console.WriteLine("No public servers found.");
                    }
                    else
                    {
#if DEBUG
                        foreach (var secret in secrets)
                        {
                            Console.WriteLine(secret);
                        }
#endif

//                    List<string>? secrets = null;
//                    try
//                    {
//                        var jsonSecrets = await new HttpClient().GetStringAsync("http://master.beattogether.systems:8989/get_public_server_secrets");
//                        secrets = JsonConvert.DeserializeObject<List<string>>(jsonSecrets);
//                    }
//                    catch (HttpRequestException ex)
//                    {
//                        if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
//                        {
//                            secrets = null;
//                        }
//                    }

//                    if (secrets == null || secrets.Count == 0)
//                    {
//                        Console.WriteLine("No public servers found.");
//                    }
//                    else
//                    {
//#if DEBUG
//                        foreach (var secret in secrets)
//                        {
//                            Console.WriteLine(secret);
//                        }
//#endif

//                        foreach (string ServerSecret in secrets)
//                        {
//                            var jsonPubServers = await new HttpClient().GetStringAsync($"http://master.beattogether.systems:8989/get_server_infomation_from_secret/{ServerSecret}");
//                            //var jsonPubServers = @"{""Name"":""BeatTogether Quickplay: All"",""Id"":""ziuMSceapEuNN7wRGQXrZg"",""EndPoint"":""199.195.251.114"",""Secret"":""wEd2K8P4kPQeTl2XMOvZeY"",""Code"":""QN7FG"",""Public"":true,""InGameplay"":false,""beatmap_level_selection_mask"":{""difficulties"":31,""modifiers"":2,""song_packs"":""/////////////////////w""},""gameplay_server_configuration"":{""max_player_count"":5,""discovery_policy"":2,""invite_policy"":2,""gameplay_server_mode"":0,""song_selection_mode"":0,""gameplay_server_control_settings"":0},""Player_count"":1}";
//                            var lobby = API.Lobby.FromJson(jsonPubServers);
//                            if (lobby != null)
//                            {
//#if DEBUG
//                                Console.WriteLine(lobby.Name);
//                                Console.WriteLine(lobby.Id);
//                                Console.WriteLine(lobby.EndPoint);
//                                Console.WriteLine(lobby.Secret);
//                                Console.WriteLine(lobby.Code);
//                                Console.WriteLine(lobby.Public);
//                                Console.WriteLine(lobby.InGameplay);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Difficulties);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.energyType);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.noFailOn0Energy);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.instaFail);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.failOnSaberClash);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.enabledObstacleType);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.fastNotes);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.strictAngles);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.disappearingArrows);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.ghostNotes);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.noBombs);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.songSpeed);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.noArrows);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.proMode);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.zenMode);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.Modifiers.smallCubes);
//                                Console.WriteLine(lobby.BeatmapLevelSelectionMask.SongPacks);
//                                Console.WriteLine(lobby.GameplayServerConfiguration.MaxPlayerCount);
//                                Console.WriteLine(lobby.GameplayServerConfiguration.DiscoveryPolicy);
//                                Console.WriteLine(lobby.GameplayServerConfiguration.InvitePolicy);
//                                Console.WriteLine(lobby.GameplayServerConfiguration.GameplayServerMode);
//                                Console.WriteLine(lobby.GameplayServerConfiguration.SongSelectionMode);
//                                Console.WriteLine(lobby.GameplayServerConfiguration.GameplayServerControlSettings);
//                                Console.WriteLine(lobby.PlayerCount);
//                                //break;
//#endif
//                                var point = PointData
//                                    .Measurement("Lobby")
//                                    .Field("Name", lobby.Name)
//                                    .Field("Id", lobby.Id)
//                                    .Tag("EndPoint", lobby.EndPoint)
//                                    .Field("Secret", lobby.Secret)
//                                    .Field("Code", lobby.Code)
//                                    .Field("Public", lobby.Public)
//                                    .Field("InGameplay", lobby.InGameplay)
//                                    //.Field("beatmap_level_selection_mask", PointData
//                                        .Tag("difficulty", lobby.BeatmapLevelSelectionMask.Difficulties.ToString())
//                                        .Field("energyType", lobby.BeatmapLevelSelectionMask.Modifiers.energyType)
//                                        .Field("noFailOn0Energy", lobby.BeatmapLevelSelectionMask.Modifiers.noFailOn0Energy)
//                                        .Field("instaFail", lobby.BeatmapLevelSelectionMask.Modifiers.instaFail)
//                                        .Field("failOnSaberClash", lobby.BeatmapLevelSelectionMask.Modifiers.failOnSaberClash)
//                                        .Field("enabledObstacleType", lobby.BeatmapLevelSelectionMask.Modifiers.enabledObstacleType)
//                                        .Field("fastNotes", lobby.BeatmapLevelSelectionMask.Modifiers.fastNotes)
//                                        .Field("strictAngles", lobby.BeatmapLevelSelectionMask.Modifiers.strictAngles)
//                                        .Field("disappearingArrows", lobby.BeatmapLevelSelectionMask.Modifiers.disappearingArrows)
//                                        .Field("ghostNotes", lobby.BeatmapLevelSelectionMask.Modifiers.ghostNotes)
//                                        .Field("noBombs", lobby.BeatmapLevelSelectionMask.Modifiers.noBombs)
//                                        .Field("songSpeed", lobby.BeatmapLevelSelectionMask.Modifiers.songSpeed)
//                                        .Field("noArrows", lobby.BeatmapLevelSelectionMask.Modifiers.noArrows)
//                                        .Field("proMode", lobby.BeatmapLevelSelectionMask.Modifiers.proMode)
//                                        .Field("zenMode", lobby.BeatmapLevelSelectionMask.Modifiers.zenMode)
//                                        .Field("smallCubes", lobby.BeatmapLevelSelectionMask.Modifiers.smallCubes)
//                                        .Field("song_packs", lobby.BeatmapLevelSelectionMask.SongPacks)
//                                    //    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
//                                    //)
//                                    .Field("maxplayercount", lobby.GameplayServerConfiguration.MaxPlayerCount)
//                                    .Field("discoverypolicy", lobby.GameplayServerConfiguration.DiscoveryPolicy)
//                                    .Field("invitepolicy", lobby.GameplayServerConfiguration.InvitePolicy)
//                                    .Field("gameplayservermode", lobby.GameplayServerConfiguration.GameplayServerMode)
//                                    .Field("songselectionmode", lobby.GameplayServerConfiguration.SongSelectionMode)
//                                    .Field("gameplayservercontrolsettings", lobby.GameplayServerConfiguration.GameplayServerControlSettings)
//                                    .Field("playercount", lobby.PlayerCount)
//                            }
//                        }
//                    }
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception trying to get or process data for InfluxDB: {ex.Message}");
                }
            }
        }
    }
}
