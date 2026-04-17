using HarmonyLib;
using PompsUwuCompany.Config;
using PompsUwuCompany.Models;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(FlashlightItem))]
    public class FlashlightItemPatch

    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(FlashlightItem.SwitchFlashlight))]
        private static void NotifyHelmetLight(FlashlightItem __instance, bool on)
        {
            Plugin.Logger!.LogDebug("trigger flashlight");
            if (!__instance.IsOwner) return;
            
            Plugin.Logger!.LogDebug("is owner!");
            if (on)
            {
                HUDManager.Instance.DisplayGlobalNotification($"wet dere be wight! {UWUController.GetRandomUWUWord(UWUWordTag.HAPPY)}");
            }
        }

        //[HarmonyPostfix]
        //[HarmonyPatch("Awake")]
        //private static void TestAwake(FlashlightItem __instance)
        //{
        //    Plugin.Logger!.LogInfo($"FlashlightItem AWAKE called for {__instance.name}");
        //}
    }
}