using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    internal class VoidOceanFloor : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_VOIDOUTROFLOOR";
        public override string SeerToken => "LAI_SEER_VOIDOCEANFLOOR";

        public override GameObject BackgroundPrefab => SceneSetup.VoidOutroSet7;

        public override Vector3 Position => new Vector3(0f, -65.65f, 13);

        public override Quaternion Rotation => Quaternion.Euler(358.0001f, 89.9999f, 349.9999f);

        public override Vector3 Scale => Vector3.one;

        //MainMenu/MENU: Multiplayer/World Position/HOLDER: Background/
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/DLC1/voidstage/matSkyboxVoidStage.mat");

        public override string MusicTrackName => "muIntroCutscene";
        //RoR2/DLC1/voidstage/matSkyboxVoidStage.mat
        //RoR2/DLC1/voidstage/matSkyboxVoid.mat
    }
}