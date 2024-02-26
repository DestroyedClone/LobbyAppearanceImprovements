﻿using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class CaptainsHelm : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_CAPTAINSHELM";
        public override GameObject BackgroundPrefab => SceneSetup.CaptainHelmObject;
        public override Vector3 Position => new Vector3(-16.7f, -3.7f, 8);
        public override Quaternion Rotation => Quaternion.Euler(0f, 180f, 0);//Quaternion.Euler(10f, 179f, 0);
        public override Vector3 Scale => Vector3.one;
        public override string PreferredLayout => nameof(Layouts.CaptainsHelm_Default);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/intro/matIntroSkybox.mat");
        //RoR2/Base/intro/matIntroSkybox.mat
        //RoR2/Base/intro/matIntroSkyboxStars.mat
        //RoR2/Base/intro/Cutscene Space Skybox.prefab
    }
}