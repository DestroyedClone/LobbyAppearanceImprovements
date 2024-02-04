using RoR2.SurvivorMannequins;

namespace LobbyAppearanceImprovements
{
    internal static class LAIMannequinManager
    {
        public static SurvivorMannequinDioramaController mannequinDioramaController = null;

        public static void Init()
        {
            On.RoR2.SurvivorMannequins.SurvivorMannequinDioramaController.OnEnable += (orig, self) =>
            {
                LAIMannequinManager.mannequinDioramaController = self;
                orig(self);
            };
        }
    }
}