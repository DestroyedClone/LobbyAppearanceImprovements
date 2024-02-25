using UnityEngine;

namespace LobbyAppearanceImprovements.Scenes
{
    public class ContactLight : LAIScene
    {
        public override string SceneNameToken => "LAI_MAP_CONTACTLIGHT";
        public override GameObject BackgroundPrefab => SceneSetup.ContactLight_Bandit2;
        public override Vector3 Position => new Vector3(0f, 0f, 3);
        public override Quaternion Rotation => Quaternion.Euler(0f, 180f, 0);
        public override Vector3 Scale => new Vector3(1f, 1f, 1f);
        public override string PreferredLayout => nameof(Layouts.Any_Empty);
        //public override Material SkyboxOverride => LoadAsset<Material>("");

        public override void Init()
        {
            base.Init();
            return;
            var tempObject = CloneFromAddressable("RoR2/Base/Common/DefaultLobbyBackground.prefab");
            Object.Destroy(tempObject.transform.Find($"Title Fog, Plane"));
            Object.Destroy(tempObject.transform.Find($"HumanCrate1Mesh"));
            Object.Destroy(tempObject.transform.Find($"HumanCanister1Mesh"));
            Object.Destroy(tempObject.transform.Find($"HumanCrate2Mesh"));
            Object.Destroy(tempObject.transform.Find($"HANDTeaser"));
            var meshProps = tempObject.transform.Find("MeshProps");

        }
    }
}