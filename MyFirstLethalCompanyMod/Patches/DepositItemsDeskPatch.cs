using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Models;
using System.Collections;
using UnityEngine;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(DepositItemsDesk))]
    public class DepositItemsDeskPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(DepositItemsDesk.TakeItemsOffCounterOnServer))]
        private static void NotifyItemsCollected(DepositItemsDesk __instance)
        {
            Plugin.Logger!.LogDebug("collect");
            SimpOverTentaclesWithDelay(__instance, 2f);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(DepositItemsDesk.FinishKillAnimation))]
        private static void NotifyPlayerAttacked(DepositItemsDesk __instance)
        {
            Plugin.Logger!.LogDebug("attack");
            SimpOverTentaclesWithDelay(__instance, 2f);
        }

        private static void SimpOverTentaclesWithDelay(DepositItemsDesk instance, float delaySeconds)
        {
            if (instance != null)
                instance.StartCoroutine(DelayedNotification(delaySeconds));
            else
                ShowNotification();
        }

        private static IEnumerator DelayedNotification(float delay)
        {
            yield return new WaitForSeconds(delay);
            ShowNotification();
        }

        private static void ShowNotification()
        {
            Plugin.Logger!.LogDebug("show notif");
            HUDManager.Instance.DisplayGlobalNotification($"w-wuz dat a... tentacwe? {UWUController.GetRandomUWUWord(UWUWordTag.BASHFUL)}");
        }
    }
}