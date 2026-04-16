using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PompsUwuCompany.Utils
{
    public class PlayerUtils
    {
        public static bool isInventoryFull(PlayerControllerB player)
        {
            if (player.ItemSlots == null) return true;

            for (int i = 0; i < player.ItemSlots.Length; i++)
            {
                if (player.ItemSlots[i] == null) return false;
            }

            return true;
        }
    }
}
