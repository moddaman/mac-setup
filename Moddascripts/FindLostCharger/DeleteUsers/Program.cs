using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

// const string url = "https://tiny-red-poodle.rmq.cloudamqp.com/api/nodes";
// const string username = "rsauzsey";
// const string password = "uuN4FCnBddxVVRJiWEC2dkviEpMVWuX8";

namespace DeleteUsers
{
    class Program
    {
        private static HttpClient _client;

        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File($"{DateTime.Now}-log.txt") // log file.
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            const string url = "https://glad-gray-pug.rmq.cloudamqp.com";
            const string username = "boxmthis";
            const string password = "Ky7Ze0G8YdXOVpwLpv-lLfCeBgn6-dIA";

            _client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            // var chargersInAWS = GetChargersFromFile("aws_export.csv");
            //  Log.Information($"Chargers in AWS {chargersInAWS.Count}");
            // IOrderedEnumerable<string> chargersInAWSSorted = chargersInAWS.OrderBy(o => o);
            var chargersInAWS = GetChargersFromFile("aws_export.csv");
            await WriteRabbitMqUsersToFile(url);
            var usersInRabbitMQ = GetChargersFromFile("usersInRabbitMQ.csv");
            Log.Information($"Users in rabbitmq {usersInRabbitMQ.Count}");
            IOrderedEnumerable<string> usersInRabbitMQSorted = usersInRabbitMQ.OrderBy(o => o);

            //
            var chargersInAwsAndRabbitMqBySort = usersInRabbitMQSorted.Intersect(chargersInAWS.OrderBy(o => o));
            IEnumerable<string> inAwsAndRabbitMqBySort = chargersInAwsAndRabbitMqBySort.ToList();
            Log.Information($"Found {inAwsAndRabbitMqBySort.Count()} chargers in rabbit and AWS iot");
            await WriteToFile("inBoth.txt", chargersInAwsAndRabbitMqBySort.ToList());
            // var deletedChargerrs = new List<string>();
            // var deletedCounter = 0;
            // var notDeletedCounter = 0;
            // foreach (var charger in inAwsAndRabbitMqBySort)
            // {
            //     var assertHasAwsIotSetAsMqHost = await AssertHasAwsIotSetAsMQHost(charger);
            //     if (!assertHasAwsIotSetAsMqHost)
            //     {
            //         continue;
            //     }
            //     var deleted = await DeleteUserFromRabbitMq(url, charger);
            //     if (deleted)
            //     {
            //         deletedChargerrs.Add(charger);
            //         deletedCounter++;
            //     }
            //     else
            //     {
            //         notDeletedCounter++;
            //     }
            //     var processed = deletedCounter + notDeletedCounter;
            //     if (processed % 100 == 0)
            //     {
            //         Console.WriteLine(
            //             $"{deletedCounter} deleted.  {notDeletedCounter} skipped. Total {chargersInAWS.Count}");
            //     }
            // }
            // Log.Information(
            //     $"{deletedCounter} deleted.  {notDeletedCounter} skipped. Total {chargersInAWS.Count}");
            // await WriteToFile("./deletedChargers", deletedChargerrs.ToList());
        }


        private static List<string> GetChargersFromFile(string path)
        {
            using var reader = new StreamReader($@"{path}");
            var chargersInAwsIot = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                chargersInAwsIot.Add(line);
            }

            Log.Information($"Got {chargersInAwsIot.Count} chargers from AWS");
            return chargersInAwsIot;
        }

        private static async Task<bool> DeleteUserFromRabbitMq(string url, string serialnumber)
        {
            string path = $"/api/users/{serialnumber}";
            var response = await _client.DeleteAsync($"{url}{path}");
            if (response.IsSuccessStatusCode)
            {
                Log.Information($"Successfully deleted user {serialnumber}");
                return true;
            }
            else
            {
                Log.Warning($"Failed to delete {serialnumber}");
                return false;
            }
        }

        private static async Task<bool> DeleteUserFromRabbitMqBulk(string url, IEnumerable<string> serialnumbers)
        {
            string path = $"/api/users/bulk-delete";
            IEnumerable<string> enumerable = serialnumbers as string[] ?? serialnumbers.ToArray();
            var serialize = JsonSerializer.Serialize(new Users(enumerable));
            var stringContent = new StringContent(serialize, Encoding.UTF8,
                "application/json");
            var response = await _client.PostAsync($"{url}{path}",
                stringContent);
            if (response.IsSuccessStatusCode)
            {
                Log.Information($"Successfully deleted user {string.Join(",", enumerable)}");
                return true;
            }
            else
            {
                Log.Warning($"Failed to delete {string.Join(",", enumerable)}");
                return false;
            }
        }

        private static async Task<bool> AssertHasAwsIotSetAsMQHost(string serialnumber)
        {
            var masterloopUrl = $"https://api.masterloop.net/api/devices/{serialnumber}/settings/expanded";
            var masterloopClient = new HttpClient();
            masterloopClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                "NaBFVdxfRc7BxoD2dndgEduQzW0gx99KHeHZCVjNcbibnnYNX9NtivSHFh5QN4utccnPVyc8MCoXWlqvAm5t2zsz7xkX3Uccu3zjifnqN8sasdc3sZF0ALM7ybeqeRSuufQ60xGHgcdL3XNIyJP6UOwZEHfDjTqcPcd9XiL9mvaDgpV5VaIeXb9DYrSeWLr2Z9k36x2BrNpaWwMUsILGt5i3DyM3Ke1COM5YbQ0-RkwxtEBpW7rtE-w-tI-G2NVSr0nuLPE1MMh-PziO8PsoSh2Dw1FiWo0i6TNeTGnayAvMzSM_QU1XRDlDQfJpfjKAG--Sms8JWmrEpu-ypQwq20NyCrVZwTP9DaQQtBY_uWQPm315wBhiox2k6I8PGv02");
            var masterloopResponse = await masterloopClient.GetAsync(masterloopUrl);
            if (masterloopResponse.IsSuccessStatusCode)
            {
                var root = await masterloopResponse.Content.ReadFromJsonAsync<Root>();
                var MQHostEaseeIoT = root?.Values.SingleOrDefault(r => r.Name == "90");
                if (MQHostEaseeIoT is not {Value: "a1u0xyvhw6w6m8-ats.iot.eu-west-1.amazonaws.com"})
                {
                    Log.Warning(
                        $"Could not delete {serialnumber} because MQHostEaseeIoT is {MQHostEaseeIoT?.Value}");
                    return false;
                }

                var IotSystem = root.Values.SingleOrDefault(r => r.Name == "IoTSystem");
                if (IotSystem is not {Value: "1"})
                {
                    Log.Warning($"Could not delete {serialnumber} because IotSystem is {IotSystem?.Value}");
                    return false;
                }

                Log.Information(
                    $"{serialnumber} is ok in masterloop. has MQHostEaseeIoT: {MQHostEaseeIoT.Value}, IoTSystem; {IotSystem.Value}");
            }

            return true;
        }

        private static async Task<bool> GetUserFromRabbitMq(string url, string serialnumber)
        {
            string path = $"/api/users/{serialnumber}";

            var response = await _client.GetAsync($"{url}{path}");
            if (response.IsSuccessStatusCode)
            {
                Log.Information($"Successfully got user {serialnumber}");
                return true;
            }
            else
            {
                Log.Warning($"Failed to delete {serialnumber}");
                return false;
            }
        }

        private static async Task<List<RabbitMQUser>> WriteRabbitMqUsersToFile(string url)
        {
            const string path = "/api/users?columns=name";

            var response = await _client.GetAsync($"{url}{path}");
            if (response.IsSuccessStatusCode)
            {
                Log.Information("Success");
                string result = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<RabbitMQUser>>(result);
                Log.Information($"Found {users.Count} in rabbitMQ");
                await WriteToFile($"./usersInRabbitMQ.csv", users.Select(u => u.name));
                return users;
            }

            Log.Information("Failed");
            throw new Exception("Could not retrieve user from rabbitMQ");
        }


        private static async Task WriteToFile(string path, IEnumerable<string> chargers)
        {
            await using var fs = File.CreateText(path);
            foreach (var charger in chargers)
            {
                await fs.WriteLineAsync(charger);
            }
        }
    }

    public class Users
    {
        public Users(IEnumerable<string> serialnumbers)
        {
            users = serialnumbers.ToList();
        }

        public List<string> users { get; set; }
    }

    public class Snapshot
    {
        public IEnumerable<string> MIDs { get; set; }
        public IEnumerable<int> ObservationIds { get; set; }
        public IEnumerable<int> SettingIds { get; set; }
    }

    // public class SnapshotResult
    // {
    //     public IEnumerable<string> MIDs { get; set; }
    //     public IEnumerable<int> ObservationIds { get; set; }
    //     public IEnumerable<int> SettingIds { get; set; }
    //
    // }
    public class SnapshotResult
    {
        public string MID { get; set; }
        public List<object> Observations { get; set; }
        public List<ValueSettings> Settings { get; set; }
        public object PulsePeriods { get; set; }
    }

    public class ValueSettings
    {
        public string Name { get; set; }
        public int DataType { get; set; }
        public bool IsDefaultValue { get; set; }
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class Root
    {
        public List<ValueSettings> Values { get; set; }
        public string MID { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }

    public class RabbitMQUser
    {
        public string name { get; set; }
    }
}