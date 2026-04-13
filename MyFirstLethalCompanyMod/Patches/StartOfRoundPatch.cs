using GameNetcodeStuff;
using HarmonyLib;
using MyFirstLethalCompanyMod.Config;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstLethalCompanyMod
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatch
    {
        private static List<PlayerControllerB> ActivePlayers
        {
            get
            {
                if (StartOfRound.Instance == null)
                    return new List<PlayerControllerB>();

                return StartOfRound.Instance.allPlayerScripts
                    .Where(p => p.isPlayerControlled || p.isPlayerDead)
                    .ToList();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(StartOfRound.OnShipLandedMiscEvents))]
        private static void DisplayStartMessage()
        {
            HUDManager.Instance.DisplayGlobalNotification($"you awe wooking vewwy cute today {UWUController.GetRandomUWUWord()}");
        }
    }
}
