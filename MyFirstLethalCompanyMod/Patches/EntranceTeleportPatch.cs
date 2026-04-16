using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Models;
using PompsUwuCompany.Utils;
using System.Collections.Generic;
using System.Linq;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(EntranceTeleport))]
    public class EntranceTeleportPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EntranceTeleport.TeleportPlayer))]
        private static void DisplayEntranceMessage()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            bool isInside = player.isInsideFactory;

            if (isInside)
                HUDManager.Instance.DisplayGlobalNotification($"it wooks so scawwy in hewe. can i hold youw hand? {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
            else
            {
                ScrapQuery inFacility = new ScrapQuery().InFactory();

                List<GrabbableObject> scrapStillInFacility = inFacility.Execute();

                if (scrapStillInFacility.Any() && !PlayerUtils.isInventoryFull(player))
                    HUDManager.Instance.DisplayGlobalNotification(
                        $"dere's still stuff in dere pookie!\ngo back and get me more monies {UWUController.GetRandomUWUWord(UWUWordTag.DEVIOUS)}"
                        );
                else if (PlayerUtils.isInventoryFull(player))
                    HUDManager.Instance.DisplayGlobalNotification($"u awe so guud at dis game {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
                else
                    HUDManager.Instance.DisplayGlobalNotification($"wowie, dat wuz a good shift. U shud head back now {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
            }
        }
    }
}
