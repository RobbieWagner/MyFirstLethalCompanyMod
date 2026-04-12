using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MyFirstLethalCompanyMod.Config;
using MyFirstLethalCompanyMod.Patches;

namespace MyFirstLethalCompanyMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // Mod Infrastructure
        internal new static ManualLogSource Logger;
        private static Harmony _harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        // Public Instance Refs
        public static Terminal _terminal;

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Custom plugin {PluginInfo.PLUGIN_NAME} ({PluginInfo.PLUGIN_GUID}) is loaded! Currently on version {PluginInfo.PLUGIN_VERSION}");
        
            ApplyAllPatches();
        }

        private static void ApplyAllPatches()
        {
            _harmony.PatchAll(typeof(ItemLoggerPatch));
            _harmony.PatchAll(typeof(CharacterPatch));
            _harmony.PatchAll(typeof(StartOfRoundPatch));
            _harmony.PatchAll(typeof(TerminalPatch));
        }

        private static void ApplyAllConfigs()
        {

        }
    }
}
