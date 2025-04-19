using System.Text.Json;

namespace AutoSeed
{
    public class Settings
    {
        public TimeSpan WeekdaySeedTime { get; set; } = new TimeSpan(10, 0, 0); // default 10:00 AM
        public TimeSpan WeekendSeedTime { get; set; } = new TimeSpan(10, 0, 0); // default 10:00 AM
        public string ServerName { get; set; } = "=ROTN= | discord.gg/rangersofthenorth | LVL 25+ AT PEAK HOURS";
        public string MainFormColor { get; set; } = "#181926";
        public string MainFormLogo { get; set; } = "AutoSeeder.png";

        public string TimeZoneId { get; set; } = "Eastern Standard Time"; // default for Helios


        public Settings AllianceSettings { get; set; }
        public Settings AussieSettings { get; set; }
        public string CurrentMode { get; set; } = string.Empty;

        public List<ServerEntry> ServerList { get; set; } = new List<ServerEntry>
        {
            new ServerEntry
            {
                Name = "Helios | Lvl 25+ | Discord.gg/newhelios",
                ApiUrl = "http://207.244.232.58:7010/api/get_public_info",
                Ip = "192.169.86.109:7787"
            }
        };

        public string ApiUrl => ServerList?.FirstOrDefault(s => s.Name == ServerName)?.ApiUrl ?? string.Empty;

        private static readonly string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public static Settings Load()
        {
            if (!File.Exists(settingsPath))
                return GetDefault();

            string json = File.ReadAllText(settingsPath);
            return JsonSerializer.Deserialize<Settings>(json) ?? GetDefault();
        }

        public static Settings GetDefault(string mode = "Alliance")
        {
            if (mode == "Aussie")
            {
                return new Settings
                {
                    WeekdaySeedTime = new TimeSpan(6, 30, 0), // 6: 30AM
                    WeekendSeedTime = new TimeSpan(6, 30, 0), // 6: 30AM
                    ServerName = "GARRYBUSTERS | discord.gg/garrybusters",
                    ServerList = new List<ServerEntry>
                    {
                        new ServerEntry
                        {
                            Name = "GARRYBUSTERS | discord.gg/garrybusters",
                            ApiUrl = "http://192.169.95.74:7010/api/get_public_info",
                            Ip = "103.193.80.145:7777"
                        }
                    },
                    MainFormColor = "#333333",
                    MainFormLogo = "Aussie_logo.jpg",
                    TimeZoneId = "AUS Eastern Standard Time"
                };
            }

            //else if (mode == "Helios")
            //{
            //    return new Settings
            //    {
            //        WeekdaySeedTime = new TimeSpan(10, 0, 0), // 10 AM
            //        WeekendSeedTime = new TimeSpan(10, 0, 0), // 10 AM
            //        ServerName = "Helios | Lvl 25+ | Discord.gg/newhelios",
            //        ServerList = new List<ServerEntry>
            //    {
            //        new ServerEntry
            //        {
            //            Name = "Helios | Lvl 25+ | Discord.gg/newhelios",
            //            ApiUrl = "http://207.244.232.58:7010/api/get_public_info",
            //            Ip = "192.169.86.109:7787"
            //        }
            //    },
            //        MainFormColor = "#181926",
            //        MainFormLogo = "AutoSeeder.png",
            //        TimeZoneId = "Eastern Standard Time"
            //    };
            //}

            // Default: Alliance Mode
            return new Settings
            {
                WeekdaySeedTime = new TimeSpan(10, 0, 0), // 10 AM
                WeekendSeedTime = new TimeSpan(10, 0, 0), // 10 AM
                ServerList = new List<ServerEntry>
                {
                    new ServerEntry
                    {
                        Name = "=ROTN= | discord.gg/rangersofthenorth | LVL 25+ AT PEAK HOURS",
                        ApiUrl = "https://rotn-stats.crcon.cc/api/get_public_info",
                        Ip = "92.118.18.90:7777"
                    },
                    new ServerEntry
                    {
                        Name = "Bakers Dozen [B13] | US East | discord.gg/b13",
                        ApiUrl = "https://b13stats.brewdawgs.vip/api/get_public_info",
                        Ip = "192.169.86.54:7777"
                    },
               new ServerEntry
                    {
                        Name = "Helios | Lvl 25+ | Discord.gg/newhelios",
                        ApiUrl = "http://207.244.232.58:7010/api/get_public_info",
                        Ip = "192.169.86.109:7787"
                    }
                },
                MainFormColor = "#181926",
                MainFormLogo = "AutoSeeder.png",
                TimeZoneId = "Eastern Standard Time"
            };
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsPath, json);
        }
    }

    public class ServerEntry
    {
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public string Ip { get; set; }
    }
}
