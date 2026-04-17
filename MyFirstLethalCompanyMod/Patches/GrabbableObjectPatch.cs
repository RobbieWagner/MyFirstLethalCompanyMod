using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Models;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(GrabbableObject))]
    public class GrabbableObjectPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GrabbableObject.GrabItem))]
        private static void NotifyItemGrabbed(GrabbableObject __instance)
        {
            if (__instance.itemProperties.isScrap)
            {
                int scrapValue = __instance.scrapValue;
                if (scrapValue >= 99)
                {
                    HUDManager.Instance.DisplayGlobalNotification($"OOOO is so shinee {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
                }
                else if (scrapValue <= 36)
                {
                    HUDManager.Instance.DisplayGlobalNotification($"Uhhh wut da hellie?\nbro dat woot sux {UWUController.GetRandomUWUWord(UWUWordTag.SAD)}");
                }
            }
        }
    }
}