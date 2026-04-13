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
        private static bool _configLoaded = false;

        public override void LoadConfig()
        {
            string configPath = Path.Combine(Plugin.ModDirectory, "Config", "uwu_words.json");

            // file path showing as incorrect, but is correct. Need to debug
            if (!File.Exists(configPath))
            {
                Plugin.Logger?.LogError($"UWU word config missing at: {configPath}");
                return;
            }
            else
                Plugin.Logger?.LogDebug($"UWU word config being loaded from: {configPath}");

                string jsonContent = File.ReadAllText(configPath);
            var loadedWords = JsonConvert.DeserializeObject<List<UWUWord>>(jsonContent);

            if (loadedWords == null || !loadedWords.Any())
            {
                Plugin.Logger?.LogError($"UWU word config is empty or invalid at: {configPath}");
                return;
            }

            words = loadedWords;
            Plugin.Logger?.LogInfo($"Loaded {words.Count} UWU words");
        }

        public static string? GetRandomUWUWord(UWUWordTag tag = UWUWordTag.NONE)
        {
            if (words == null || !words.Any())
            {
                Plugin.Logger?.LogWarning("No UWU words loaded - returning 'uwu'");
                return "uwu";
            }

            var validWords = words
                .Where(word => tag == UWUWordTag.NONE || word.tags.Contains(tag))
                .Select(word => word.word)
                .Where(w => !string.IsNullOrEmpty(w))
                .ToList();

            if (!validWords.Any())
            {
                Plugin.Logger?.LogWarning($"No words with tag {tag} - returning 'uwu'");
                return "uwu";
            }

            return validWords[new Random().Next(validWords.Count)];
        }
    }
}