using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace FindLostChargers
{
    class Program
    {
        private static HttpClient _client;

        private static async Task<ChargerPulse> CheckCharger(string chargerId)
        {
            try
            {
                var masterloopUrl = $"https://api.masterloop.net/api/devices/{chargerId}/pulse/0/current";

                var tsToWhiteListResult = await _client.GetAsync(masterloopUrl);
                if (tsToWhiteListResult.IsSuccessStatusCode)
                {
                    var parse = await tsToWhiteListResult.Content.ReadFromJsonAsync<ChargerPulse>();
                    parse.Mid = chargerId;
                    parse.FoundIntMasterloop = true;
                    return parse;
                }

                return new ChargerPulse
                {
                    Mid = chargerId,
                    FoundIntMasterloop = false
                };
            }
            catch (Exception e)
            {
                return new ChargerPulse
                {
                    Mid = chargerId,
                    FoundIntMasterloop = false,
                    Error = false,
                };
            }
        }

        private static async Task GetChargers()
        {
            using var reader = new StreamReader(@"./DistinctMidsHistorian010621.csv");
            var chargersWithNewObservedObservations = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                chargersWithNewObservedObservations.Add(line);
            }

            Console.WriteLine($"{chargersWithNewObservedObservations.Count} chargers with observations in historian");
            reader.Dispose();
            using var otherReader = new StreamReader(@"./AllCharger.csv");
            var chargersWithoutHistorianData = new List<string>();
            var counter = 0;
            while (!otherReader.EndOfStream)
            {
                var line = await otherReader.ReadLineAsync();∫
                if (line == null) continue;
                counter++;
                string[] chargerWithCreatedDate = line.Split(",");
                var chargerMid = chargerWithCreatedDate[0];
                var existsInHistorian = chargersWithNewObservedObservations.Any(c => c == chargerMid);
                if (!existsInHistorian)
                {
                    chargersWithoutHistorianData.Add(
                        chargerMid
                    );
                }

                if (counter % 100 == 0)
                {
                    Console.WriteLine($"{counter} chargers checked");
                }
            }

            Console.WriteLine($"{chargersWithoutHistorianData.Count} chargers without observations in historian");
            await WriteToFile($"./ChargersWithoutHistorianData.csv", chargersWithoutHistorianData);
        }

        private static async Task OldStuff()
        {
            // const string token =
            //     "BmNKvqHkh0NkvMTohjM0E3GHHV5k9aLznMd-JVLKJa3OJ7ElonuybMjkoB20NX2uvBYWEMmu8rRz4UN8Q00VgmOg73IuJ50mbATWOhFZ35BouKu4wxA3ZUUhghBk_1Mzuam9rbV2G3auJwAs_fNA0qw1A10PDFNhTXfYj7bZ_HkThn7GUUi5AkZ_8gQD5kHbR9B_7dM6Ac6vkqCMNpAUA8do59w7lQ21N2S8GUqXVhAFu06ILtuBLyWYDTU9cN8FoznQPtE7UqYFOsQLQtJ-DV4-a_vNi_ePCXpuTwkhWB99DuDIVWVxqXt4Rs6AC53d9p7tPYEJFse12lhnhyQpXTBXiMh6GcwyUH9lxZmW3dLxgl75CasEF4dq47AybbYp";
            //
            // string[] chargersWithNewObservedObservations = Array.Empty<string>();
            // while (!reader.EndOfStream)
            // {
            //     var line = await reader.ReadLineAsync();
            //     if (line != null) chargersWithNewObservedObservations = line.Split(',');
            // }
            //
            // _client = new HttpClient();
            // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // var counter = 0;
            // await using var fs = File.CreateText(@"./MissingChargersData1.csv");
            //
            // foreach (var charger in chargersWithNewObservedObservations)
            // {
            //     var t = await CheckCharger(charger);
            //     await fs.WriteLineAsync(t.ToString());
            //     counter++;
            //     if (counter % 100 == 0)
            //     {
            //         Console.WriteLine($"{counter} chargers checked");
            //         Thread.Sleep(1);
            //     }
            // }
            //
            // Console.WriteLine("Finished");
        }

        static async Task Main(string[] args)
        {
            var x = DateTime.Now.ToString("O");
            var path = $"./Report-{x}";
            System.IO.Directory.CreateDirectory(path);
            using var reader = new StreamReader(@"./GetAlldevicesWIthMetadata-1622708872019.json");
            var text = await File.ReadAllTextAsync(@"./GetAlldevicesWIthMetadata-1622708872019.json");

            var tryALlDevices =
                JsonSerializer.Deserialize<IEnumerable<Root>>(text);
            IEnumerable<Root> aLlDevices = (tryALlDevices ?? new List<Root>()).ToList();

            Console.WriteLine($"Found {aLlDevices.Count()} devices");

            var onlyFromTemplate = aLlDevices.Where(a => a.TemplateId == "EASEECHG").ToList();
            Console.WriteLine($"{onlyFromTemplate.Count()} devices belongs to template EASEECHG");


            List<Root> noPulse1 = onlyFromTemplate.Where(c => c.LatestPulse == null).ToList();
            List<Root> pulse7DaysAfterCreated1 = onlyFromTemplate
                .Where(c => c.LatestPulse.HasValue && c.LatestPulse.Value > c.CreatedOn.AddDays(7)).ToList();
            List<Root> pulse7DaysBeforeCreated1 = onlyFromTemplate
                .Where(c => c.LatestPulse.HasValue && c.LatestPulse.Value <= c.CreatedOn.AddDays(7)).ToList();

            await WriteOverview($"{path}/overview.csv", onlyFromTemplate);
            await WriteObjectsToFile($"{path}/NoPulseSent.csv", noPulse1);
            await WriteObjectsToFile($"{path}/pulse7DaysAfterCreated.csv", pulse7DaysAfterCreated1);
            await WriteObjectsToFile($"{path}/pulseOnly7DaysBeforeCreated.csv", pulse7DaysBeforeCreated1);

            //Chargers created GroupedBy year/month, count, Pulse 7 days after created, NO pulse
            IOrderedEnumerable<IGrouping<(int Year, int Month), Root>> chargersGroupedByMonth =
                onlyFromTemplate.GroupBy(r => (r.CreatedOn.Year, r.CreatedOn.Month)).OrderBy(t => t.Key.Year)
                    .ThenBy(xy => xy.Key.Month);
            Console.WriteLine(
                $"Date,  Chargers created, Chargers with pulse sent 7 days after created, Chargers with pulse only sent 7 days before created, No pulse sent");
            await WriteObjectsToFiles($"{path}/overview-grouped-by-month.csv", chargersGroupedByMonth);


            foreach (IGrouping<(int Year, int Month), Root> grouping in chargersGroupedByMonth)
            {
                var monthYearPath = $"{path}/{grouping.Key.Year}-{grouping.Key.Month}";
                System.IO.Directory.CreateDirectory(monthYearPath);
                List<Root> noPulse = grouping.Where(c => c.LatestPulse == null).ToList();
                List<Root> pulse7DaysAfterCreated = grouping
                    .Where(c => c.LatestPulse.HasValue && c.LatestPulse.Value > c.CreatedOn.AddDays(7)).ToList();
                List<Root> pulse7DaysBeforeCreated = grouping
                    .Where(c => c.LatestPulse.HasValue && c.LatestPulse.Value <= c.CreatedOn.AddDays(7)).ToList();
                Console.WriteLine(
                    $"{grouping.Key.Year}-{grouping.Key.Month}, {grouping.Count()}, {pulse7DaysAfterCreated.Count}, {pulse7DaysBeforeCreated.Count}, {noPulse.Count}");
                await WriteObjectsToFile($"{monthYearPath}/NoPulseSent.csv", noPulse);
                await WriteObjectsToFile($"{monthYearPath}/pulse7DaysAfterCreated.csv", pulse7DaysAfterCreated);
                await WriteObjectsToFile($"{monthYearPath}/pulseOnly7DaysBeforeCreated.csv", pulse7DaysBeforeCreated);
            }

            Console.WriteLine("FInished");
        }


        public class Root
        {
            public DateTime CreatedOn { get; set; }
            public DateTime UpdatedOn { get; set; }
            public DateTime? LatestPulse { get; set; }
            public string TemplateId { get; set; }
            public string MID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; } = "null";
            public object Metadata { get; set; }

            public override string ToString()
            {
                if (LatestPulse.HasValue)
                {
                    return $"{MID},{CreatedOn},{UpdatedOn},{LatestPulse.Value},{Name},{Description},{TemplateId}";
                }

                return $"{MID},{CreatedOn},{UpdatedOn},Pulse missing,{Name},{Description},{TemplateId}";
            }
        }

        public class ChargerPulse
        {
            public string Mid { get; set; }
            public int MaximumAbsence { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public int PulseCount { get; set; }
            public bool FoundIntMasterloop { get; set; }

            public bool Error { get; set; } = false;

            public string ToString()
            {
                return $"{Mid},{From},{To}, {PulseCount}, {FoundIntMasterloop}, {MaximumAbsence}, {Error}";
            }
        }


        private static async Task WriteOverview(string path, List<Root> list)
        {
            await using StreamWriter file = new StreamWriter(path);
            await file.WriteLineAsync(
                "Total chargers, Pulse sent 7 days after created, Pulse only sent before 7 days, No pulse sent");

            List<Root> noPulse = list.Where(c => c.LatestPulse == null).ToList();
            IEnumerable<Root> pulse7DaysAfterCreated =
                list.Where(c => c.LatestPulse.HasValue && c.LatestPulse.Value > c.CreatedOn.AddDays(7));
            IEnumerable<Root> pulse7DaysBeforeCreated =
                list.Where(c => c.LatestPulse.HasValue && c.LatestPulse.Value <= c.CreatedOn.AddDays(7));
            await file.WriteLineAsync(
                $"{list.Count}, {pulse7DaysAfterCreated.Count()}, {pulse7DaysBeforeCreated.Count()}, {noPulse.Count}");
        }


        private static async Task WriteObjectsToFiles(string path,
            IOrderedEnumerable<IGrouping<(int Year, int Month), Root>> groupings)
        {
            await using StreamWriter file = new StreamWriter(path);
            await file.WriteLineAsync(
                "Created, Total chargers, Pulse sent 7 days after created, Pulse only sent before 7 days, No pulse sent");

            foreach (IGrouping<(int Year, int Month), Root> grouping in groupings)
            {
                List<Root> noPulse = grouping.Where(c => c.LatestPulse == null).ToList();
                IEnumerable<Root> pulse7DaysAfterCreated = grouping.Where(c =>
                    c.LatestPulse.HasValue && c.LatestPulse.Value > c.CreatedOn.AddDays(7));
                IEnumerable<Root> pulse7DaysBeforeCreated = grouping.Where(c =>
                    c.LatestPulse.HasValue && c.LatestPulse.Value <= c.CreatedOn.AddDays(7));
                await file.WriteLineAsync(
                    $"{grouping.Key.Year}-{grouping.Key.Month}, {grouping.Count()}, {pulse7DaysAfterCreated.Count()}, {pulse7DaysBeforeCreated.Count()}, {noPulse.Count}");
            }
        }

        private static async Task WriteObjectsToFile(string path, IEnumerable<Root> chargers)
        {
            await using StreamWriter file = new StreamWriter(path);
            await file.WriteLineAsync("MID,CreatedOn,UpdatedOn,LatestPulse,Name,Description,TemplateId");
            foreach (var charger in chargers)
            {
                await file.WriteLineAsync(charger.ToString());
            }
        }

        private static async Task WriteToFile(string path, IEnumerable<string> chargersWithBindings)
        {
            await using var fs = File.CreateText(path);
            await fs.WriteLineAsync(string.Join(",", chargersWithBindings));
        }
    }
}