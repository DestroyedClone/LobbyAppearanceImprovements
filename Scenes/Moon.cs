using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class Moon : LAIScene
    {
        public override string SceneName => "Commencement";
        public override GameObject BackgroundPrefab => SetupDiorama();
        public override Vector3 Position => new Vector3(0, -4.6f, 25);
        public override Quaternion Rotation => Quaternion.Euler(0f, 90f, 0);
        public override Vector3 Scale => new Vector3(1, 1f, 1);
        public static GameObject MoonDioramaFinal = null;

        public GameObject SetupDiorama()
        {
            var diorama = Resources.Load<GameObject>("prefabs/stagedisplay/MoonDioramaDissplay"); ;
            if (!MoonDioramaFinal)
            {
                diorama.transform.Find("MoonBridgeCornerWithTerrain");
            }
            return diorama;
        }
    }
}