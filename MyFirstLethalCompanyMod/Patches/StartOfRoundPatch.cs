using DunGen.Tags;
using GameNetcodeStuff;
using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class StartOfRoundPatch
    {
        private static bool? _doorState = null;

        private static float hangarTimer = 0;

        private static bool isPlayingJingle = false;
        private static bool hasPlayedJingleThisRound = false;
        private static float timeToJingle = 20;
        private static float jingleTimer = 0;
        private static int currentBar = 0;
        private static List<string> bars = new() 
        {
            "dis wittle wight of mine! ",
            "im gunna wet it shine ",
            "dis wittle wight of mine! ",
            "im gunna wet it shine ",
            "dis wittle wight of mine! ",
            "im gunna wet it shine ",
            "wet it shine, wet it shine ",
            "wet it shiiiiinneeeeee! "
        };
        private static int secondsPerBar = 3;

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

            hasPlayedJingleThisRound = false;
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

        [HarmonyPostfix]
        [HarmonyPatch(nameof(StartOfRound.Update))]
        private static void OnUpdate(StartOfRound __instance)
        {
            UpdateHangarTimer(__instance);
            UpdateJingleTimer(__instance);
        }

        private static void UpdateJingleTimer(StartOfRound instance)
        {
            if (hasPlayedJingleThisRound || !isPlayingJingle || IsJingleComplete())
            {
                jingleTimer = 0;
                return;
            }

            jingleTimer += Time.deltaTime;
            //Plugin.Logger?.LogDebug($"jingle timer: {jingleTimer}");
        }

        private static bool IsJingleComplete()
        {
            if (jingleTimer >= bars.Count * secondsPerBar)
            {
                hasPlayedJingleThisRound = true;
                return true;
            }

            if (jingleTimer > currentBar * secondsPerBar)
            {
                HUDManager.Instance.DisplayGlobalNotification(bars[currentBar] + UWUController.GetRandomUWUWord(UWUWordTag.HAPPY));
                currentBar++;
            }

            return false;
        }

        private static void UpdateHangarTimer(StartOfRound __instance)
        {
            if (!__instance.shipHasLanded || !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom || isPlayingJingle)
            {
                hangarTimer = 0;
                return;
            }

            hangarTimer += Time.deltaTime;
            // Plugin.Logger?.LogDebug($"hangar timer: {hangarTimer}/{timeToJingle}");

            if (hangarTimer >= timeToJingle)
            {
                isPlayingJingle = true;
            }
        }
    }
}
