using LobbyAppearanceImprovements.Scenes;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LobbyAppearanceImprovements.Layouts
{
    public class Any_ROR2 : LAILayout
    {
        public override string SceneLayout => "Any_ROR2";
        public override string SceneName => "Dynamic";
        public override string Author => "DestroyedClone";
        public override string LayoutNameToken => "LAI_LAYOUT_ROR2";
        public override string ReadmeDescription => "Only the characters from RoR2, but for each stage";

        public override void Init()
        {
            base.Init();
            LAILayout.onLayoutLoaded += OnLayoutLoaded;
            LAIScene.onSceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(LAIScene scene)
        {
            if (LAILayoutManager.chosenLayout == null || LAILayoutManager.layoutInstance == null)
                return;
            OnLayoutLoaded(LAILayoutManager.chosenLayout);
        }

        private void OnLayoutLoaded(LAILayout layout)
        {
            if (!layout.IsLayoutOfType<Any_ROR2>()) return;
            string chosenSceneAsString = LAISceneManager.chosenSceneAsString;
            Debug.Log($"We're in Any_RoR2 as the scene: {chosenSceneAsString}");
            switch (chosenSceneAsString)
            {
                case "eclipse":
                case "lobbyvoid":
                    chosenSceneAsString = "lobbyvoid";
                    break;
            }
            if (!sceneCharacterLayouts.TryGetValue(chosenSceneAsString, out var positionsDict))
                return;
            //Debug.Log($"sceneCharacterLayouts has our chosenSceneAsString, {LAISceneManager.chosenSceneAsString}");
            foreach (var survivorDisplay in InstanceTracker.GetInstancesList<LAILayout.LAI_CharacterDisplay>())
            {
                //Debug.Log($"Found survivorDisplay for {survivorDisplay.survivorDef.cachedName}");
                var bodyName = survivorDisplay.survivorDef.bodyPrefab.name;
                //Debug.Log($"bodyName: {bodyName}");
                if (bodyName.EndsWith("Body"))
                    bodyName = bodyName.Substring(0, bodyName.Length - 4);
                //Debug.Log($"new bodyName: {bodyName}");
                if (!positionsDict.TryGetValue(bodyName, out Vector3[] values))
                {
                    var defaultSet = sceneCharacterLayouts["lobby"];
                    if (defaultSet.TryGetValue(bodyName, out values))
                    {
                        goto UseDefaultValues;
                    }
                    continue;
                }
            UseDefaultValues:
                //Debug.Log($"positionsDict found");
                survivorDisplay.gameObject.transform.position = values[0];
                survivorDisplay.gameObject.transform.rotation = Quaternion.Euler(values[1]);
            }
        }

        public static Vector3[] EmptyVector3 = new Vector3[] { Vector3.zero, Vector3.zero };

        public override Dictionary<string, Vector3[]> CharacterLayouts => new Dictionary<string, Vector3[]>()
        {
            { "Commando", EmptyVector3 },
            { "Bandit2", EmptyVector3 },
            { "Huntress", EmptyVector3 },
            { "Toolbot", EmptyVector3 },
            { "Engi", EmptyVector3 },
            { "Mage", EmptyVector3 },
            { "Merc", EmptyVector3 },
            { "Treebot", EmptyVector3 },
            { "Loader", EmptyVector3 },
            { "Croco", EmptyVector3 },
            { "Captain", EmptyVector3 },
            { "Railgunner", EmptyVector3 },
            { "VoidSurvivor", EmptyVector3 },
        };

        public static Dictionary<string, Dictionary<string, Vector3[]>> sceneCharacterLayouts = new Dictionary<string, Dictionary<string, Vector3[]>>()
        {
            { "ancientloft", new Dictionary<string, Vector3[]>()
                {
                { "Captain", new Vector3[] {new Vector3(0.21f, 0.01f, 19.4f), new Vector3(0f, 0.9961947f, 0f)} },
                { "Engi", new Vector3[] {new Vector3(-2.58f, -0.01f, 15f), new Vector3(0f, 0.9659258f, 0f)} },
                { "Huntress", new Vector3[] {new Vector3(2.100001f, -0.1700004f, 16.16f), new Vector3(0f, 0.9961947f, 0f)} },
                { "Merc", new Vector3[] {new Vector3(-1.22f, 0f, 16.28f), new Vector3(0f, 1f, 0f)} },
                { "Railgunner", new Vector3[] {new Vector3(0.8999999f, 0f, 16.5774f), new Vector3(0f, 0.9799247f, 0f)} },
                { "Treebot", new Vector3[] {new Vector3(-4.01f, -0.11f, 14.93f), new Vector3(0f, 0.9396926f, 0f)} },
                { "Toolbot", new Vector3[] {new Vector3(-5.21f, 0.15f, 12.84f), new Vector3(0f, 0.9396926f, 0f)} },
                }
            },
            { "arena", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "artifactworld", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "blackbeach", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "captainshelm", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "codes", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "eclipse", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "dampcave", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "foggyswamp", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "frozenwall", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "goldshores", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "golemplains", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "goolake", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "lakes", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "limbo", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "lobby", new Dictionary<string, Vector3[]>()
                {
                    { "Commando", new [] {new Vector3(2.65f, 0.01f, 6.00f), new Vector3(0f, 240f, 0f) } },
                    { "Bandit2", new [] {new Vector3(3.79f, 0.01f, 11.5f), new Vector3(0f, 240f, 0f) } },
                    { "Huntress", new [] {new Vector3(4.8f, 1.43f, 15.36f), new Vector3(0f, 170f, 0f) } },
                    { "Toolbot", new [] {new Vector3(-0.21f, 0.15f, 20.84f), new Vector3(0f, 170f, 0f) } },
                    { "Engi", new [] {new Vector3(-2.58f, -0.01f, 19f), new Vector3(0f, 150f, 0f) } },
                    { "Mage", new [] {new Vector3(3.35f, 0.21f, 14.73f), new Vector3(0f, 220f, 0f) } },
                    { "Merc", new [] {new Vector3(-1.32f, 3.65f, 22.28f), new Vector3(0f, 180f, 0f) } },
                    { "Treebot", new [] {new Vector3(-6.51f, -0.11f, 22.93f), new Vector3(0f, 140f, 0f) } },
                    { "Loader", new [] {new Vector3(5.04f, 0, 14.26f), new Vector3(0f, 220f, 0f) } },
                    { "Croco", new [] {new Vector3(5f, 3.59f, 22f), new Vector3(0f, 210f, 0f) } },
                    { "Captain", new [] {new Vector3(2.21f, 0.01f, 19.40f), new Vector3(0f, 190f, 0f) } },
                    { "Railgunner", new [] {new Vector3(1.3f, 3.6595f, 22.5774f), new Vector3(0f, 203f, 0f) } },
                    { "VoidSurvivor", new [] {new Vector3(3f, 0f, 5.5f), new Vector3(0f, 290f, 0f) } },
                }
            },
            { "lobbyvoid", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "moon", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "mysteryspace", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "rescueship", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "rootjungle", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "shipgraveyard", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "skymeadow", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "snowyforest", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "sulfurpools", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "voidoceanfloor", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "voidraid", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "voidstage", new Dictionary<string, Vector3[]>()
                {

                }
            },
            { "wispgraveyard", new Dictionary<string, Vector3[]>()
                {

                }
            }
        };
    }
}