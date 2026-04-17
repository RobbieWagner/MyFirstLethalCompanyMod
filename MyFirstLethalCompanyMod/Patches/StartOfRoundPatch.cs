using DunGen.Tags;
using GameNetcodeStuff;
using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Models;
using System.Collections.Generic;
using System.Linq;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatch
    {
        private static bool? _doorState = null;

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

        [HarmonyPostfix]
        [HarmonyPatch(nameof(StartOfRound.SetShipDoorsClosed))]
        private static void NotifyOfSafety(StartOfRound __instance)
        {
            if (!__instance.shipHasLanded || !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom)
                return;

            if (_doorState == __instance.hangarDoorsClosed)
                return;

            _doorState = __instance.hangarDoorsClosed;

            if (__instance.hangarDoorsClosed)
            {
                HUDManager.Instance.DisplayGlobalNotification($"it wooks so cozy in thewe. can i join? {UWUController.GetRandomUWUWord(UWUWordTag.DEVIOUS)}");
            }
            else if(!__instance.hangarDoorsClosed)
            {
                HUDManager.Instance.DisplayGlobalNotification(
                    $"oh no te doow opened {UWUController.GetRandomUWUWord(UWUWordTag.DEVIOUS)}\nbe cawefuw out thewe pwincess {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}"
                    );
            }
        }
    }
}
