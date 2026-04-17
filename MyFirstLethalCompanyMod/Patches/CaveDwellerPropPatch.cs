using HarmonyLib;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(CaveDwellerPhysicsProp))]
    public class CaveDwellerPropPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(CaveDwellerPhysicsProp.EquipItem))]
        private static void NotifyBaby()
        {
            HUDManager.Instance.DisplayGlobalNotification("Ew a baby :/");
        }
    }
}
