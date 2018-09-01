using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace MHWorldDataSharpnessConverter
{
    class Program
    {
        public struct Durability
        {
            [JsonProperty("red")]
            public int Red { get; set; }
            [JsonProperty("orange")]
            public int Orange { get; set; }
            [JsonProperty("yellow")]
            public int Yellow { get; set; }
            [JsonProperty("green")]
            public int Green { get; set; }
            [JsonProperty("blue")]
            public int Blue { get; set; }
            [JsonProperty("white")]
            public int White { get; set; }

            public int[] ToArray()
            {
                return new int[] { Red, Orange, Yellow, Green, Blue, White };
            }
        }

        public struct Weapon
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("durability")]
            public Durability[] Durabilities { get; set; }
        }

        static void Main(string[] args)
        {
            new Program().Run();
        }

        private void Run()
        {
            const string url = "https://mhw-db.com/weapons";
            var httpClient = new HttpClient();

            Console.WriteLine($"Fetchign data from '{url}'");

            string content = httpClient.GetStringAsync(url).Result;

            IEnumerable<Weapon> weapons = JsonConvert.DeserializeObject<Weapon[]>(content)
                .Where(x => x.Type != "light-bowgun" && x.Type != "heavy-bowgun" && x.Type != "bow");

            string outputFilename = Path.Combine(AppContext.BaseDirectory, "weapon_sharpness.csv");

            Console.WriteLine($"Writing to '{outputFilename}'");

            using (var fs = new FileStream(outputFilename, FileMode.Create, FileAccess.Write))
            {
                var writer = new StreamWriter(fs);

                writer.WriteLine("\"base_name_en\",\"sharpness\",\"sharpness_1\",\"sharpness_2\",\"sharpness_3\",\"sharpness_4\",\"sharpness_5\"");

                foreach (Weapon weapon in weapons)
                {
                    if (weapon.Durabilities == null || weapon.Durabilities.Length != 6)
                        Console.WriteLine($"Weapon '{weapon.Name}' has missing or invalid durability value.");
                    else
                    {
                        string allDurabilities = string.Join("\",\"", weapon.Durabilities.Select(d => string.Join(",", d.ToArray())));
                        writer.WriteLine($"\"{weapon.Name.Replace("\"", "\"\"")}\",\"{allDurabilities}\"");
                    }
                }

                writer.Flush();
            }
        }
    }
}
