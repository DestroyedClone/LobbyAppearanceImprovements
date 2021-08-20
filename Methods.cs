using UnityEngine;
using RoR2;

//using static LobbyAppearanceImprovements.StaticValues;

namespace LobbyAppearanceImprovements
{
    public static class Methods
    {
        public static void ChangeLobbyLightColor(Color32 color)
        {
            GameObject.Find("Directional Light").gameObject.GetComponent<Light>().color = color;
        }
        public static GameObject CreateDisplay(string bodyPrefabName, Vector3 position, Vector3 rotation, Transform parent = null)
        {
            var bodyPrefab = GetBodyPrefab(bodyPrefabName);

            SurvivorDef survivorDef = SurvivorCatalog.FindSurvivorDefFromBody(bodyPrefab);
            GameObject displayPrefab = survivorDef.displayPrefab;
            var gameObject = UnityEngine.Object.Instantiate<GameObject>(displayPrefab, position, Quaternion.Euler(rotation), parent);
            switch (bodyPrefabName)
            {
                case "Croco":
                    gameObject.transform.Find("mdlCroco")?.transform.Find("Spawn")?.transform.Find("FloorMesh")?.gameObject.SetActive(false);
                    break;
                case "RobEnforcer":
                    break;
                case "HANDOverclocked":
                    GameObject.Find("HANDTeaser").SetActive(false);
                    break;
            }
            return gameObject;
        }

        public static GameObject GetBodyPrefab(string bodyPrefabName)
        {
            switch (bodyPrefabName)
            {
                case "CHEF":
                    break;
                default:
                    bodyPrefabName += "Body";
                    break;
            }
            var bodyPrefab = BodyCatalog.FindBodyPrefab(bodyPrefabName);
            if (!bodyPrefab) return null;
            return bodyPrefab;
        }
    }
}