using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LobbyAppearanceImprovements.Scenes
{
    public class CoolerEclipseLobby : LAIScene
    {
        public override string SceneNameToken => "LAI_MOD_COOLERECLIPSELOBBY";
        public override GameObject BackgroundPrefab => eclipseLobby;
        public override Vector3 Position => new Vector3(0f, 0f, 0);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override Material SkyboxOverride => LoadAsset<Material>("RoR2/Base/mysteryspace/matSkyboxMysterySpace.mat"); //guessed

        public static GameObject eclipseLobby;

        public override string Credit => "Nuxlar";

        public override void Init()
        {
            base.Init();
            GameObject eclipseWeather = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/eclipseworld/Weather, Eclipse.prefab").WaitForCompletion();
            eclipseLobby = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerRun.prefab").WaitForCompletion().GetComponent<InfiniteTowerRun>().lobbyBackgroundPrefab, "LAI_CoolerEclipseLobbyNux", false);


            eclipseLobby.transform.GetChild(6).gameObject.SetActive(true);
            eclipseLobby.transform.GetChild(6).GetChild(0).gameObject.SetActive(true);

            eclipseLobby.transform.GetChild(7).gameObject.SetActive(false);
            eclipseLobby.transform.GetChild(8).gameObject.SetActive(false);
            eclipseLobby.transform.GetChild(9).GetChild(11).gameObject.SetActive(false);
            eclipseLobby.transform.GetChild(9).GetChild(12).gameObject.SetActive(false);
            eclipseLobby.transform.GetChild(11).gameObject.SetActive(false);

            GameObject lobbyWeather = GameObject.Instantiate(eclipseWeather, eclipseLobby.transform);
            lobbyWeather.transform.GetChild(2).GetComponent<SetAmbientLight>().ApplyLighting();
            lobbyWeather.transform.GetChild(1).gameObject.SetActive(false);

            Transform eclipse = lobbyWeather.transform.GetChild(3).GetChild(2);
            eclipse.gameObject.SetActive(true);
            eclipse.localPosition = new Vector3(-0.6f, 0f, -0.95f);
            eclipse.localEulerAngles = new Vector3(0f, 270f, 0f);
            eclipse.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            Transform eclipseRing = eclipse.GetChild(0);
            eclipseRing.localPosition = Vector3.zero;
            eclipseRing.localEulerAngles = new Vector3(0f, 120f, 0f);

        }
    }
}
