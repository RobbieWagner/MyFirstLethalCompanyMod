using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Models;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(Landmine))]
    public class LandminePatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Landmine.Detonate))]
        private static void NotifyExplosion()
        {
            HUDManager.Instance.DisplayGlobalNotification($"hehe wandmine go kaboom {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
        }
    }
}
