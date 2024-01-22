using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class LobbyInfiniteTower : LAIScene
    {
        public override string SceneName => "Lobby";
        public override string SceneNameToken => "LAI_MAP_LOBBY_INFINITETOWER";
        public override GameObject BackgroundPrefab => LoadAsset("RoR2/DLC1/Common/LobbyBackgrounds/InfiniteTowerLobbyBackground.prefab");
        public override Vector3 Position => new Vector3(3.5f, -1.5f, 3);
        public override Quaternion Rotation => Quaternion.Euler(0f, 0f, 0);
        public override Vector3 Scale => new Vector3(1f, 0.5f, 1f);
    }
}