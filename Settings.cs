using System.Text.Json;

namespace AutoSeed
{
    public class Settings
    {
        private static readonly string SettingsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "settings.json");

        // Default settings
        public TimeSpan WeekdaySeedTime { get; set; } = new TimeSpan(20, 0, 0); // 8 PM
        public TimeSpan WeekendSeedTime { get; set; } = new TimeSpan(19, 0, 0); // 7 PM
        public string ServerName { get; set; } = "GARRYBUSTERS | discord.gg/garrybusters"; // Default to Garrybusters
        public string TimeZoneId { get; set; } = "Eastern Standard Time";
        public string MainFormColor { get; set; } = "#4A5D50"; // Grayish-green color
        public string MainFormLogo { get; set; } = "Aussie_logo.jpg"; // Always use Aussie logo
        public List<ServerEntry> ServerList { get; set; } = new List<ServerEntry>();

        // Static methods to save/load settings
        public static Settings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var settings = JsonSerializer.Deserialize<Settings>(json);

                    // Always enforce Aussie mode with new grayish-green theme
                    settings.MainFormLogo = "Aussie_logo.jpg";
                    settings.MainFormColor = "#4A5D50";

                    // Filter to only include GARRY servers
                    settings.ServerList = settings.ServerList
                        .Where(server => server.Name.Contains("GARRY"))
                        .ToList();

                    // If no GARRY servers found, add default ones
                    if (settings.ServerList.Count == 0)
                    {
                        settings.ServerList = CreateDefaultGarryServer();
                    }

                    return settings;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading settings: " + ex.Message);
            }

            return CreateDefaultSettings();
        }

        public void Save()
        {
            try
            {
                // Always enforce Aussie mode with new theme
                MainFormLogo = "Aussie_logo.jpg";
                MainFormColor = "#4A5D50"; // Grayish-green color

                // Filter to only include GARRY servers
                ServerList = ServerList
                    .Where(server => server.Name.Contains("GARRY"))
                    .ToList();

                // Ensure we have at least one GARRY server
                if (ServerList.Count == 0)
                {
                    ServerList = CreateDefaultGarryServer();
                }

                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving settings: " + ex.Message);
            }
        }

        private static List<ServerEntry> CreateDefaultGarryServer()
        {
            var servers = new List<ServerEntry>();

            servers.Add(new ServerEntry
            {
                Name = "GARRYBUSTERS | discord.gg/garrybusters",
                Ip = "103.193.80.145:7777",
                ApiUrl = "http://192.169.95.74:7010/api/get_public_info"
            });

            servers.Add(new ServerEntry
            {
                Name = "GARRYBUSTERS 2 | discord.gg/garrybusters",
                Ip = "103.193.80.145:7787",
                ApiUrl = "http://192.169.95.74:7012/api/get_public_info"
            });

            return servers;
        }

        private static Settings CreateDefaultSettings()
        {
            var settings = new Settings();

            // Add default GARRYBUSTERS server
            settings.ServerList = CreateDefaultGarryServer();

            return settings;
        }

        public static void ResetToDefaults()
        {
            var defaults = CreateDefaultSettings();
            defaults.Save();
        }
    }

    public class ServerEntry
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public string ApiUrl { get; set; }
    }
}