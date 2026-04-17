using HarmonyLib;
using PompsUwuCompany.Config;

namespace PompsUwuCompany.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    public class EnemyAIPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.KillEnemy))]
        private static void NotifyKill(EnemyAI __instance, bool destroy = false)
        {
            if (!__instance.enemyType.canDie) 
                return;

            HUDManager.Instance.DisplayGlobalNotification($"An enemy dieded!\nU guys r so stwong {UWUController.GetRandomUWUWord(Models.UWUWordTag.HAPPY)}");
        }
    }
}
