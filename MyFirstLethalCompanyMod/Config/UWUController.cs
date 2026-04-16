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

            try
            {
                string? jsonContent = null;
                string? usedPath = null;

                string configPath = Path.Combine(Paths.ConfigPath, "uwu_words.json");
                Plugin.Logger!.LogDebug($"Checking standard config path: {configPath}");

                if (File.Exists(configPath))
                {
                    jsonContent = File.ReadAllText(configPath);
                    usedPath = configPath;
                }
                if (jsonContent == null)
                {
                    Plugin.Logger?.LogError($"UWU word config missing! Tried all paths.");
                    Plugin.Logger?.LogError($"Expected location: {Path.Combine(Paths.ConfigPath, "uwu_words.json")}");
                    return;
                }

                Plugin.Logger?.LogDebug($"Loading UWU config from: {usedPath}");

                List<UWUWord>? loadedWords = JsonConvert.DeserializeObject<List<UWUWord>>(jsonContent);

                if (loadedWords == null || !loadedWords.Any())
                {
                    Plugin.Logger?.LogError($"UWU word config is empty or invalid");
                    return;
                }

                words = loadedWords;
                configLoaded = true;
                Plugin.Logger?.LogInfo($"Loaded {words.Count} UWU words from config");
            }
            catch (JsonException ex)
            {
                Plugin.Logger?.LogError($"JSON parsing error: {ex.Message}");
                Plugin.Logger?.LogError($"Check your JSON format at: {Path.Combine(Paths.ConfigPath, "uwu_words.json")}");
            }
            catch (Exception ex)
            {
                Plugin.Logger?.LogError($"Failed to load UWU config: {ex.Message}");
                Plugin.Logger?.LogError($"Stack trace: {ex.StackTrace}");
            }
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
    }
}