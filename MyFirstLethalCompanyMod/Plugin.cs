using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MyFirstLethalCompanyMod.Config;
using MyFirstLethalCompanyMod.Patches;
using MyFirstLethalCompanyMod.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

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

            try 
            {
                var types = Assembly.GetExecutingAssembly().GetTypes();
                foreach (var type in types)
                {
                    var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    foreach (var method in methods)
                    {
                        var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                        if (attributes.Length > 0)
                        {
                            method.Invoke(null, null);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogDebug($"Exception while executing Netcode setup {e.Message}");
                return;
            }
        }

        private void InitializeNetworkSingletons()
        {
            // Create notifier (get initializes)
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