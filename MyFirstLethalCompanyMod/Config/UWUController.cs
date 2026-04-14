using BepInEx;
using HarmonyLib;
using MyFirstLethalCompanyMod.Models;
using MyFirstLethalCompanyMod.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyFirstLethalCompanyMod.Config
{
    public class UWUController : HarmonySingleton<UWUController>
    {
        protected override Type TargetClass => typeof(StartOfRound);
        protected override string TargetMethodName => nameof(StartOfRound.StartGame);

        public static List<UWUWord> words { get; private set; } = new List<UWUWord>();

        public override void LoadConfig()
        {
            try
            {
                string configPath = Path.Combine(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, "uwu_words.json");
                if (!File.Exists(configPath))
                {
                    configPath = Path.Combine(Plugin.ModDirectory, "Config", "uwu_words.json");
                }

                if (!File.Exists(configPath))
                {
                    Plugin.Logger?.LogError($"UWU word config missing at: {configPath}");
                    return;
                }

                string jsonContent = File.ReadAllText(configPath);
                List<UWUWord>? loadedWords = JsonConvert.DeserializeObject<List<UWUWord>>(jsonContent);

                if (loadedWords == null || !loadedWords.Any())
                {
                    Plugin.Logger?.LogError($"UWU word config is empty or invalid");
                    return;
                }

                words = loadedWords;
            }
            catch (JsonException ex)
            {
                Plugin.Logger?.LogError($"JSON parsing error: {ex.Message}");
                Plugin.Logger?.LogError($"Check your JSON format at: {Path.Combine(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, "uwu_words.json")}");
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