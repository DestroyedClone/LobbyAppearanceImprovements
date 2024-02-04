using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyAppearanceImprovements.Modules
{
    public static class Mannequins
    {
        public static void Init()
        {
            On.RoR2.SurvivorMannequins.SurvivorMannequinDioramaController.SetSlots += SurvivorMannequinDioramaController_SetSlots;
        }

        private static void SurvivorMannequinDioramaController_SetSlots(On.RoR2.SurvivorMannequins.SurvivorMannequinDioramaController.orig_SetSlots orig, RoR2.SurvivorMannequins.SurvivorMannequinDioramaController self, RoR2.SurvivorMannequins.SurvivorMannequinSlotController[] newMannequinSlots)
        {
            orig(self, newMannequinSlots);

        }
    }
}
