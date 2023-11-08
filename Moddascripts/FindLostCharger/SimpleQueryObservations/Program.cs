using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleQueryObservations
{

    class Program
    {
        private const string MasterloopToken =
            "RxaCVqSwAkV8G-9_ffJBh75F22Wcuy37S_c93olQAsE2KNIwEHmNEUmcmshXErkITc20tT3WPMjX5ezuKqyfkLycuXWBBtWkLdA79nsSqOmGALqfPsdoRjFBVlADRcaEpZ8Lc44jhPLGbA3xGRnTpCBmchKg6asayZDL7lCEdxuOvAZ8j5PYmkmaFI3h8zaznNX0bAN9oXkoyKiyYHAdUT67XsitaJBjdmug5LSt1ku88kLje4_rdfUol817-lDg4m3Di76QDHV8E3kmWdrO98qTfoH_d_CzPPpI_CaxH9mqtHIrOR8qjaaeMYbCoKpl-zFdKyVToZij3QoasLlhwITjz53KtIiyQQp_OshGkhvlsjDsP4zCZBVWHTEJeRnm";


        static async Task Main(string[] args)
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
            //Put observationIds you want to check here (think 150 is temp max)
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