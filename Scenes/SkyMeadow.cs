﻿using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class SkyMeadow : LAIScene
    {
        public override string SceneNameToken => "MAP_SKYMEADOW";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/Base/skymeadow/SkyMeadowDioramaDisplay.prefab");
        public override Vector3 Position => new Vector3(0f, -3f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0, 0, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/skymeadow/matSMSkybox.mat");
        //RoR2/Base/skymeadow/matSMSkybox.mat
        //RoR2/Base/skymeadow/matSMSkyboxMoon.mat
    }
}