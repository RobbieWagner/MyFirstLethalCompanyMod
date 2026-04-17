using GameNetcodeStuff;
using HarmonyLib;
using PompsUwuCompany.Config;
using System;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class PlayerControllerBPatch
    {
        private static int stepsTaken = 0;
        private static int annoySteps = 11;
        private static int stepsToNextAnnoy = 0;
        private static Random random = new Random();

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControllerB.ConnectClientToPlayerObject))]
        private static void OnAwake(PlayerControllerB __instance)
        {
            if (__instance != StartOfRound.Instance.localPlayerController)
                return;
            
            RestartStepper();
        }

        private static void RestartStepper()
        {
            stepsToNextAnnoy = random.Next(250, 550);
            Plugin.Logger!.LogDebug($"steps to take: {stepsToNextAnnoy}");
            stepsTaken = 0;
            annoySteps = random.Next(7, 12);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControllerB.PlayFootstepLocal))]
        private static void NotifyStep(PlayerControllerB __instance)
        {
            if (__instance != StartOfRound.Instance.localPlayerController)
                return;

            stepsTaken++;

            if (stepsTaken > stepsToNextAnnoy + annoySteps)
            {
                HUDManager.Instance.DisplayGlobalNotification($"ok sowwy iw stop now {UWUController.GetRandomUWUWord(Models.UWUWordTag.HAPPY)}");
                RestartStepper();
            }
            else if (stepsTaken > stepsToNextAnnoy)
            {
                HUDManager.Instance.DisplayGlobalNotification($"step {UWUController.GetRandomUWUWord(Models.UWUWordTag.HAPPY)}");
            }

            Plugin.Logger!.LogDebug($"step: {stepsTaken}");
        }
    }
}