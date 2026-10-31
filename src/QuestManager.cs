using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BullshotLauncher
{
    /// <summary>
    /// Represents a quest type.
    /// </summary>
    public enum QuestType
    {
        Daily,
        Social,
        Weekly
    }

    /// <summary>
    /// Represents a single quest.
    /// </summary>
    public class Quest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public QuestType Type { get; set; }
    }

    /// <summary>
    /// Manages quests for Bullshot Token Launcher.
    /// Quest state is persisted to a local JSON file.
    /// </summary>
    public class QuestManager
    {
        private readonly List<Quest> _quests = new List<Quest>();
        private readonly string _storagePath;

        public QuestManager(string? storagePath = null)
        {
            _storagePath = storagePath ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "BullshotLauncher",
                "quests.json");
            Load();
        }

        /// <summary>
        /// Returns all quests.
        /// </summary>
        public List<Quest> GetQuests() => _quests;

        /// <summary>
        /// Toggles completion status for a quest by name.
        /// </summary>
        public void ToggleQuest(string questName)
        {
            var quest = _quests.Find(q => q.Name.Equals(questName, StringComparison.OrdinalIgnoreCase));
            if (quest != null)
            {
                quest.IsCompleted = !quest.IsCompleted;
                Save();
            }
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(_storagePath))
                {
                    SeedDefaultQuests();
                    Save();
                    return;
                }

                var json = File.ReadAllText(_storagePath);
                var loaded = JsonSerializer.Deserialize<List<Quest>>(json);
                if (loaded != null)
                    _quests.AddRange(loaded);
            }
            catch
            {
                // Ignore load failures, start with defaults
                SeedDefaultQuests();
            }
        }

        private void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(_storagePath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(_quests, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_storagePath, json);
            }
            catch
            {
                // Ignore save failures
            }
        }

        private void SeedDefaultQuests()
        {
            _quests.Add(new Quest
            {
                Name = "Track First Token",
                Description = "Track the progress of your first token on Bonding Curve.",
                Type = QuestType.Daily,
                IsCompleted = false
            });
            _quests.Add(new Quest
            {
                Name = "Calculate Creator Fee",
                Description = "Use the fee calculator to estimate creator earnings.",
                Type = QuestType.Social,
                IsCompleted = false
            });
            _quests.Add(new Quest
            {
                Name = "Weekly Review",
                Description = "Review your tracked tokens at the end of the week.",
                Type = QuestType.Weekly,
                IsCompleted = false
            });
        }
    }
}