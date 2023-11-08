using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateBatch
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = Encoding.ASCII.GetBytes("85008b45-1d4c-e810-8edf-2e996c78a2b2");
            Console.WriteLine(key);
            //  var batchNumber = 1;
            // const int @from = 1;
            // const int to = 49791;
            //
            // var batchNumber2 = 2;
            // const int @from2 = 49809;
            // const int to2 = 99804;
            //
            // var batchNumber3 = 3;
            // const int @from3 = 99812;
            // const int to3 = 149815;
            //
            // var batchNumber4 = 4;
            // const int @from4 = 149823;
            // const int to4 = 199836;
            //
            // List<string> allChargers = GetChargers();
            //
            // ToBatches(@from, to, allChargers, batchNumber);
            // ToBatches(@from2, to2, allChargers, batchNumber2);
            // ToBatches(@from3, to3, allChargers, batchNumber3);
            // ToBatches(@from4, to4, allChargers, batchNumber4);

        }

        private static void ToBatches(int @from, int to, List<string> allChargers, int batchNumber)
        {
            var list = new List<string>();
            var minorBatch = 0;
            for (var i = @from; i <= to; i++)
            {
                var mid = $"EH{i:D6}";
                if (allChargers.Any(c => c == mid))
                {
                    list.Add(mid);
                }

                if (i % 2000 == 0)
                {
                    Console.WriteLine($"Added {list.Count} devices to batch");
                    WriteToFile($"./betaBatches/{batchNumber}-{minorBatch}BetaBatch.json", list);
                    list = new List<string>();
                    minorBatch++;
                }
            }

            WriteToFile($"./betaBatches/{batchNumber}-{minorBatch}BetaBatch.json", list);
            Console.WriteLine("Finished");
        }

        private static List<string> GetChargers()
        {
            using var reader = new StreamReader(@"./Charger.csv");
            var allChargers = new List<string>();
            var line = reader.ReadLine();
            reader.Dispose();
            if (line != null)
            {
                var splitted = line.Split(",");
                Console.WriteLine($"{splitted.Length} chargers");
                return splitted.ToList();
            }

            throw new Exception("dsadasdsadas");
        }

        private static void WriteToFile(string path, IEnumerable<string> chargersWithBindings)
        {
            using var fs = File.CreateText(path);
            fs.Write(string.Join(",", chargersWithBindings));
        }
    }
}