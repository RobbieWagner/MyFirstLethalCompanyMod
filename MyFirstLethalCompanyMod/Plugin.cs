using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MyFirstLethalCompanyMod.Config;
using MyFirstLethalCompanyMod.Patches;
using MyFirstLethalCompanyMod.Utils;
using System.IO;

namespace MyFirstLethalCompanyMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // Mod Infrastructure
        internal new static ManualLogSource? Logger;
        private static Harmony _harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        public static string ModDirectory { get; private set; } = "";

        // Public Instance Refs
        public static Terminal? _terminal;

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Custom plugin {PluginInfo.PLUGIN_NAME} ({PluginInfo.PLUGIN_GUID}) is loaded! Currently on version {PluginInfo.PLUGIN_VERSION}");

            ModDirectory = Path.GetDirectoryName(Info.Location);

            InitializeNetworkSingletons();
            ApplyAllPatches();
        }

        private void InitializeNetworkSingletons()
        {
            // Create notifier (get initializes)
            Notifier notifier = Notifier.Instance!;
            Logger?.LogInfo("Network singletons initialized");
        }

        private static void ApplyAllPatches()
        {
            // Singletons
            _harmony.PatchAll(typeof(HarmonySingleton<UWUController>));

            // Other patches
            _harmony.PatchAll(typeof(StartOfRoundPatch));
            _harmony.PatchAll(typeof(TerminalPatch));
        }
    }
}