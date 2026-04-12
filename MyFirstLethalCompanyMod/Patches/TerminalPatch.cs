using HarmonyLib;
using MyFirstLethalCompanyMod.Config;

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
		[HarmonyPatch(nameof(Terminal.BeginUsingTerminal))]
		private static void NotifyPlayersOfTerminalUse()
		{
			HUDManager.Instance.DisplayGlobalNotification($"Te Tewminawl is in use {UWUController.GetRandomUWUWord()}");
		}

		[HarmonyPostfix]
		[HarmonyPatch(nameof(Terminal.QuitTerminal))]
		private static void NotifyPlayersOfTerminalFree()
		{
			HUDManager.Instance.DisplayGlobalNotification($"Te Tewminawl is fwee {UWUController.GetRandomUWUWord()}");
		}
	}
}
