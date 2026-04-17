using BepInEx;
using GameNetcodeStuff;
using PompsUwuCompany.Models;
using PompsUwuCompany.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PompsUwuCompany.Config
{
    public class UWUController : HarmonySingleton<UWUController>
    {
        protected override Type TargetClass => typeof(PlayerControllerB);
        protected override string TargetMethodName => nameof(PlayerControllerB.ConnectClientToPlayerObject);

        public static List<UWUWord> words { get; private set; } = new List<UWUWord>();

        public override void LoadConfig()
        {
            if (configLoaded)
                return;

            LoadUWUWords();
        }

        public static string? GetRandomUWUWord(UWUWordTag tag = UWUWordTag.NONE)
        {
            if (words == null || !words.Any())
            {
                Plugin.Logger?.LogWarning("No UWU words loaded - returning 'uwu'");
                return "uwu";
            }

            List<string?> validWords = words
                .Where(word => tag == UWUWordTag.NONE || word.tags.Contains(tag))
                .Select(word => word.word)
                .Where(w => !string.IsNullOrEmpty(w))
                .ToList();

            if (!validWords.Any())
            {
                Plugin.Logger?.LogWarning($"No words with tag {tag} - returning 'uwu'");
                return "uwu";
            }

            Random random = new Random();
            return validWords[random.Next(validWords.Count)];
        }

        private static void LoadUWUWords()
        {
            try
            {
                string configPath = Path.Combine(Paths.ConfigPath, "uwu_words.json");

                if (!File.Exists(configPath))
                {
                    Plugin.Logger?.LogError($"UWU word config missing at: {configPath}");
                    return;
                }

                string jsonContent = File.ReadAllText(configPath);
                words = JsonConvert.DeserializeObject<List<UWUWord>>(jsonContent)!;
                words.RemoveAll(w => string.IsNullOrWhiteSpace(w.word));

                configLoaded = true;
                Plugin.Logger?.LogInfo($"Loaded {words.Count} UWU words from config");
                if (!words.Any())
                    Plugin.Logger?.LogWarning($"No uwu words were loaded. All replacements will be \"uwu\"");
            }
            catch (Exception ex)
            {
                Plugin.Logger?.LogError($"Failed to load UWU config: {ex.Message}");
            }
        }
    }
}