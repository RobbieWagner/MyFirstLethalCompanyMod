using DunGen.Tags;
using GameNetcodeStuff;
using HarmonyLib;
using MyFirstLethalCompanyMod.Config;
using MyFirstLethalCompanyMod.Models;
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
            HUDManager.Instance.DisplayGlobalNotification($"you awe wooking vewwy cute today {UWUController.GetRandomUWUWord(UWUWordTag.BASHFUL)}");
        }

        // TODO: Fix so it doesnt trigger on ship enter
        [HarmonyPostfix]
        [HarmonyPatch(nameof(StartOfRound.SetPlayerSafeInShip))]
        private static void NotifyOfSafety(StartOfRound __instance)
        {
            if (__instance.hangarDoorsClosed && GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom && __instance.shipHasLanded)
            {
                HUDManager.Instance.DisplayGlobalNotification($"it wooks so cozy in thewe. can i join? {UWUController.GetRandomUWUWord(UWUWordTag.DEVIOUS)}");
            }
            else if(!__instance.hangarDoorsClosed && GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom && __instance.shipHasLanded)
            {
                HUDManager.Instance.DisplayGlobalNotification($"oh no te doow opened {UWUController.GetRandomUWUWord(UWUWordTag.DEVIOUS)}\nbe cawefuw out thewe pwincess {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
            }
        }
    }
}
