using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChargersOnlineAfterProduced
{
    class MyEqualityComparer : IEqualityComparer<Charger>
    {
        public bool Equals(Charger x, Charger y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.SerialNumber == y.SerialNumber;
        }

        public int GetHashCode(Charger obj)
        {
            return HashCode.Combine(obj.SerialNumber);
        }
    }

    public class Charger
    {
        public string SerialNumber { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LatestTempMax { get; set; }
    }

    public class D
    {
        public int year { get; set; }

        public int month { get; set; }
    }

    class Program
    {
        private const string MasterloopToken =
            "ABC";

        static async Task Main(string[] args)
        {
            var filename = $"Online-Result-Current.csv";
            var chargersAlreadyProcessed = GetAlreadyProcessedChargerResultsFromFile(filename).ToList();

            IOrderedEnumerable<IGrouping<(int Year, int Month), ChargerResult>> chargersGroupedByCreated =
                chargersAlreadyProcessed.GroupBy(r => (r.Created.Year, r.Created.Month))
                    .OrderBy(t => t.Key.Year)
                    .ThenBy(xy => xy.Key.Month);

            var summaryfilename = $"Online-Result-Summary.csv";
            // if (!File.Exists(summaryfilename))
            await using StreamWriter file = new StreamWriter(summaryfilename);
            // }

            await using StreamWriter sw = File.AppendText(summaryfilename);
            foreach (IGrouping<(int Year, int Month), ChargerResult> chargerResults in chargersGroupedByCreated)
            {
                var resultForYear = new ResultForYear(chargerResults);
                await sw.WriteLineAsync(
                    $"{resultForYear.CreatedYear}-{resultForYear.CreatedMonth},{resultForYear.TotalChargers},{resultForYear.OnlineAfterProduction}");
            }

            Console.WriteLine("Done");
        }

        static async Task Main2x(string[] args)
        {
            var listOfChargers = new List<string> {"EH062687"};
            Console.WriteLine($"Got {listOfChargers.Count()} chargers");
            var masterloopUrl =
                $"https://api.masterloop.net/api/tools/snapshot/current";

            var masterloopClient = new HttpClient();
            masterloopClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                MasterloopToken);

            const string filename = "Filename.csv";
            if (!File.Exists(filename))
            {
                await using StreamWriter file = new StreamWriter(filename);
            }
            //Put observationIds you want to check here
            var observationIds = new[] {150};

            await using StreamWriter sw = File.AppendText(filename);
            await sw.WriteLineAsync($"Serialnumber,TempMax,date");
            foreach (var charger in listOfChargers)
            {
                try
                {
                    var serialize = JsonSerializer.Serialize(new SnapshotRequest
                    {
                        MIDs = new[] {charger},
                        ObservationIds = observationIds,
                    });
                    var stringContent = new StringContent(serialize, Encoding.UTF8,
                        "application/json");
                    var masterloopResponse = await masterloopClient.PostAsync(masterloopUrl,
                        stringContent);
                    if (masterloopResponse.IsSuccessStatusCode)
                    {
                        var root = await masterloopResponse.Content
                            .ReadFromJsonAsync<IEnumerable<SnapshotResult>>();
                        var curr = root.FirstOrDefault();
                        if (curr == null)
                        {
                            throw new Exception("Found in masterloop, but is null");
                        }

                        if (curr.Observations == null)
                        {
                            throw new Exception("Found in masterloop, but has never sent tempMax");
                        }

                        var firstObservation = curr.Observations.FirstOrDefault();
                        var firstObservationDate = firstObservation?.Timestamp;
                        if (firstObservationDate == null)
                        {
                            throw new Exception($"tempMaxObsAsDate didn´t have timestamp");
                        }



                        await sw.WriteLineAsync(
                            $"{charger},{firstObservation.Value},{firstObservationDate.Value}");
                    }
                    else
                    {
                        throw new Exception("Not found in masterloop");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to get for {charger}");
                    await sw.WriteLineAsync(
                        $"{charger},-1, -1");
                }
            }
        }

        private static List<ChargerResult> GetAlreadyProcessedChargerResultsFromFile(string path)
        {
            using var reader = new StreamReader($@"./{path}");
            var chargers = new List<ChargerResult>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                chargers.Add(new ChargerResult(line));
            }

            return chargers;
        }

        private static List<string> GetAlreadyProcessedChargersFromFile(string path)
        {
            using var reader = new StreamReader($@"{path}");
            var chargers = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splitted = line.Split(",");
                chargers.Add(splitted[0]);
            }

            return chargers;
        }

        private static List<Charger> GetChargersFromFile(string path)
        {
            using var reader = new StreamReader($@"{path}");
            var chargers = new List<Charger>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splitted = line.Split(",");
                chargers.Add(new Charger
                {
                    SerialNumber = splitted[0],
                    Created = DateTime.Parse(splitted[1]),
                    LatestTempMax = null,
                });
            }

            return chargers;
        }
    }

    internal class ResultForYear
    {
        public ResultForYear(IGrouping<(int Year, int Month), ChargerResult> chargerResults)
        {
            CreatedYear = chargerResults.Key.Year;
            CreatedMonth = chargerResults.Key.Month;
            TotalChargers = chargerResults.Count();
            OnlineAfterProduction = 0;
            foreach (var chargerResult in chargerResults)
            {
                if (chargerResult.PreviousTempMaxSentDate.HasValue)
                {
                    var daysbetween = (chargerResult.PreviousTempMaxSentDate.Value - chargerResult.Created).Days;
                    if (daysbetween > 7)
                    {
                        OnlineAfterProduction++;
                    }
                }
            }
        }

        public int CreatedYear { get; set; }
        public int CreatedMonth { get; set; }
        public int TotalChargers { get; set; }
        public int OnlineAfterProduction { get; set; }
    }

    public class ChargerResult
    {
        public ChargerResult(string line)
        {
            var splitted = line.Split(",");
            SerialNumber = splitted[0];
            var result = DateTime.TryParse(splitted[1], out var parsedDate);
            if (!result)
            {
                Created = DateTime.MinValue;
            }
            else
            {
                Created = parsedDate;
            }

            var maybeLastTempDate = DateTime.TryParse(splitted[2], out var tempMax);
            if (maybeLastTempDate)
            {
                PreviousTempMaxSentDate = tempMax;
            }
            else
            {
                PreviousTempMaxSentDate = null;
            }
        }

        public string SerialNumber { get; set; }
        public DateTime Created { get; set; }
        public DateTime? PreviousTempMaxSentDate { get; set; }
    }

    public class SnapshotRequest
    {
        public string[] MIDs { get; set; }
        public int[] ObservationIds { get; set; }
    }

    public class SnapshotResult
    {
        public string MID { get; set; }
        public List<Observation> Observations { get; set; }
        public List<object> Settings { get; set; }
        public object PulsePeriods { get; set; }
    }

    public class Observation
    {
        public int DataType { get; set; }
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Value { get; set; }
    }
}