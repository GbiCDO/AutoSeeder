using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoSeed
{
    public class Settings
    {
        public TimeSpan WeekdaySeedTime { get; set; } = new TimeSpan(10, 0, 0); // default 10:00 AM
        public TimeSpan WeekendSeedTime { get; set; } = new TimeSpan(10, 0, 0); // default 10:00 AM
        public string ServerName { get; set; } = "Helios | Lvl 25+ | Discord.gg/newhelios";
        public string MainFormColor { get; set; } = "#181926";
        public string MainFormLogo { get; set; } = "Helios Logo TRANSPARENT.png";

        public string TimeZoneId { get; set; } = "Eastern Standard Time"; // default for Helios

        public Settings HeliosSettings { get; set; }
        public Settings AussieSettings { get; set; }
        public string CurrentMode { get; set; } = string.Empty;

        public List<string> ServerList { get; set; } = new List<string>
        {
            "Helios | Lvl 25+ | Discord.gg/newhelios"
        };

        private static readonly string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public static Settings Load()
        {
            if (!File.Exists(settingsPath))
                return GetDefault();

            string json = File.ReadAllText(settingsPath);
            return JsonSerializer.Deserialize<Settings>(json) ?? GetDefault();
        }

        public static Settings GetDefault(string mode = "Helios")
        {
            if (mode == "Aussie")
            {
                return new Settings
                {
                    WeekdaySeedTime = new TimeSpan(6, 30, 0), // 6: 30AM
                    WeekendSeedTime = new TimeSpan(6, 30, 0), // 6: 30AM
                    ServerName = "GARRYBUSTERS | discord.gg/garrybusters",
                    ServerList = new List<string> { "GARRYBUSTERS | discord.gg/garrybusters" },
                    MainFormColor = "#333333",
                    MainFormLogo = "Aussie_logo.jpg",
                    TimeZoneId = "AUS Eastern Standard Time"
                };
            }

            // Default: Helios Mode
            return new Settings
            {
                WeekdaySeedTime = new TimeSpan(10, 0, 0), // 10 AM
                WeekendSeedTime = new TimeSpan(10, 0, 0), // 10 AM
                ServerName = "Helios | Lvl 25+ | Discord.gg/newhelios",
                ServerList = new List<string> { "Helios | Lvl 25+ | Discord.gg/newhelios" },
                MainFormColor = "#181926",
                MainFormLogo = "Helios Logo TRANSPARENT.png",
                TimeZoneId = "Eastern Standard Time"
            };
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsPath, json);
        }
    }
}
