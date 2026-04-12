using GameNetcodeStuff;
using HarmonyLib;

namespace MyFirstLethalCompanyMod
{
	[HarmonyPatch(typeof(PlayerControllerB))]
	public class CharacterPatch
	{
		[HarmonyPostfix]
		[HarmonyPatch("Awake")]
		private static void AdjustPlayerJumpForce(PlayerControllerB __instance)
		{
            __instance.jumpForce = 150;
		}
	}
}
