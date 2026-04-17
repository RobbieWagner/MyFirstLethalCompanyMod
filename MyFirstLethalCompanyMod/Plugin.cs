using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Patches;
using PompsUwuCompany.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace PompsUwuCompany
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
            ModDirectory = Path.GetDirectoryName(Info.Location);

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

            Logger.LogInfo($"Custom plugin {PluginInfo.PLUGIN_NAME} ({PluginInfo.PLUGIN_GUID}) is loaded! Currently on version {PluginInfo.PLUGIN_VERSION}");
        }

        private static void ApplyAllPatches()
        {
            // Singletons
            try
            {
                _harmony.PatchAll(typeof(HarmonySingleton<UWUController>));
            }
            catch (Exception e)
            {
                Logger!.LogWarning($"Could not patch {typeof(HarmonySingleton<UWUController>)}: {e.Message}");
            }

            // Other patches
            List<Type> notificationPatches = new ()
            {
                typeof(StartOfRoundPatch),
                typeof(TerminalPatch),
                typeof(EntranceTeleportPatch),
                typeof(GrabbableObjectPatch),
                typeof(CaveDwellerPropPatch),
                typeof(LandminePatch),
                typeof(FlashlightItemPatch),
                typeof(DepositItemsDeskPatch),
                typeof(EnemyAIPatch),
                typeof(PlayerControllerBPatch)
            };

            foreach (Type patch in notificationPatches)
            {
                try
                {
                    _harmony.PatchAll(patch);
                    Logger!.LogDebug($"Patch {patch} applied successfully");
                }
                catch (Exception e)
                {
                    Logger!.LogWarning($"Could not patch {patch}: {e.Message}");
                    continue;
                }
            }
        }
    }
}