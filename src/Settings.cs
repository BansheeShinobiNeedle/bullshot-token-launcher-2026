using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace BullshotLauncher
{
    /// <summary>
    /// Application settings persisted to JSON.
    /// </summary>
    public class Settings
    {
        public List<string> TrackedTokens { get; set; } = new List<string>();
        public string DisplayTheme { get; set; } = "Dark";
        public bool ShowLiquidityLocked { get; set; } = true;

        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BullshotLauncher",
            "settings.json");

        /// <summary>
        /// Loads settings from disk. Returns defaults if file does not exist.
        /// </summary>
        public static Settings Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return new Settings();

                var json = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
            }
            catch
            {
                return new Settings();
            }
        }

        /// <summary>
        /// Saves settings to disk.
        /// </summary>
        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch
            {
                // Silently ignore save failures
            }
        }
    }
}