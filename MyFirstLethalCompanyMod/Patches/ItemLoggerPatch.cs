using HarmonyLib;

namespace MyFirstLethalCompanyMod
{
    [HarmonyPatch(typeof(GrabbableObject))]
    public class ItemLoggerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GrabbableObject.EquipItem))]
        private static void DebugItemName(GrabbableObject __instance)
        {
            string itemName = __instance.itemProperties.itemName;
            Plugin.Logger?.LogDebug(itemName);
        }
    }
}
