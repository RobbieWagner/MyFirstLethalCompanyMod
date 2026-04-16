using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Models;

namespace PompsUwuCompany.Patches
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
            Plugin.Logger?.LogDebug($"notify of terminal use (client side) {inUse}");
            if (inUse)
            {
                HUDManager.Instance.DisplayGlobalNotification($"Te Tewminawl is in use {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
            }
            else
            {
                HUDManager.Instance.DisplayGlobalNotification($"Te Tewminawl is fwee {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Terminal.SetTerminalInUseServerRpc))]
        private static void NotifyPlayersOfTerminalFreeServer(bool inUse)
        {
            Plugin.Logger?.LogDebug($"notify of terminal use (server side) {inUse}");
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
