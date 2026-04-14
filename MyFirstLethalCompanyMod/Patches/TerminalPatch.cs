using HarmonyLib;
using MyFirstLethalCompanyMod.Config;
using MyFirstLethalCompanyMod.Models;
using MyFirstLethalCompanyMod.Utils;

namespace MyFirstLethalCompanyMod.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    public class TerminalPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Terminal.Awake))]
        private static void CaptureTerminal(Terminal __instance)
        {
            Plugin._terminal = __instance;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.SetTerminalInUseClientRpc))]
        private static void NotifyPlayersOfTerminalFreeClient(bool inUse)
        {
            Plugin.Logger?.LogDebug("notify of terminal use");
            if (inUse)
            {
                HUDManager.Instance.DisplayGlobalNotification($"Te Tewminawl is in use {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
            }
            else
            {
                HUDManager.Instance.DisplayGlobalNotification($"Te Tewminawl is fwee {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
            }
        }
    }
}
