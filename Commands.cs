using RoR2;
using System;
using UnityEngine;
using static LobbyAppearanceImprovements.Methods;
using System.Collections.Generic;
using System.Linq;

namespace LobbyAppearanceImprovements
{
    public static class Commands
    {
        [ConCommand(commandName = "LAI_SpawnPrefab", flags = ConVarFlags.SenderMustBeServer, helpText = "path x y z")]
        public static void CMD_SpawnPrefab(ConCommandArgs args)
        {
            var path = args.GetArgString(0);
            var gay = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(path).WaitForCompletion();
            var diorama = (GameObject)UnityEngine.Object.Instantiate(gay);
            diorama.transform.position = new Vector3(args.GetArgFloat(1), args.GetArgFloat(2), args.GetArgFloat(3));
        }

        //[ConCommand(commandName = "LAI_bar", flags = ConVarFlags.SenderMustBeServer, helpText = "path x y z")]
        public static void CMD_SpawnRestraintBar(ConCommandArgs args)
        {
            var cum = UnityEngine.Object.Instantiate(SceneSetup.ContactLight_RestraintBar, RoR2Content.Survivors.Commando.displayPrefab.transform);
        }

        [ConCommand(commandName = "LAI_ListScenes", flags = ConVarFlags.None, helpText = "Lists the available scenes.")]
        public static void CMD_ListScene(ConCommandArgs args)
        {
            foreach (var keyValuePair in LAISceneManager.scenesDict)
            {
                Debug.Log(keyValuePair.Key);
            }
        }

        [ConCommand(commandName = "LAI_SetScene", flags = ConVarFlags.None, helpText = "Sets the current scene to the specified name. | For previewing, does not save.")]
        public static void CMD_SetScene(ConCommandArgs args)
        {
            if (args.Count == 1)
            {
                SelectScene(args.GetArgString(0));
            }
            else
            {
                Methods.LoadSceneAndLayout(args.GetArgString(0), args.GetArgString(1));
            }
        }

        [ConCommand(commandName = "LAI_ListLayouts", flags = ConVarFlags.None, helpText = "lai_listlayouts - shows all layouts." +
            "\n lai_listlayouts {sceneName} - shows all layouts for a particular scene")]
        public static void CMD_ListLayouts(ConCommandArgs args)
        {
            if (args.Count == 1)
            {
                var sceneName = args.GetArgString(0);
                foreach (var keyValuePair in LAILayoutManager.layoutsDict)
                {
                    var instance = (Layouts.LAILayout)Activator.CreateInstance(keyValuePair.Value);
                    if (instance.SceneName == sceneName)
                        Debug.Log(keyValuePair.Key);
                }
            }
            else
            {
                foreach (var keyValuePair in LAILayoutManager.layoutsDict)
                {
                    Debug.Log(keyValuePair.Key);
                }
            }
        }

        [ConCommand(commandName = "LAI_SetLayout", flags = ConVarFlags.None, helpText = "Sets the current layout to the specified name. | For previewing, does not save.")]
        public static void CMD_SetLayout(ConCommandArgs args)
        {
            SelectLayout(args.GetArgString(0));
        }

        [ConCommand(commandName = "LAI_getpos", flags = ConVarFlags.None, helpText = "lai_getpos | Returns bodyname, pos, rotation, in format for scenelayout file")]
        public static void CMD_GetPos(ConCommandArgs args)
        {
            var bodyName = args.senderBody.name;
            // https://stackoverflow.com/questions/15564944/remove-the-last-three-characters-from-a-string
            bodyName = bodyName.Remove(bodyName.Length - 7);
            var pos = args.senderBody.footPosition;
            var rot = args.senderBody.transform.rotation;
            var text = "{ \"" + bodyName + "\", new [] {new Vector3(" + pos.x + "f, " + pos.y + "f, " + pos.z + "f), new Vector3(" + rot.x + ", " + rot.y + "f, " + rot.z + "f) } },";
            Debug.Log(text);
        }

        [ConCommand(commandName = "LAI_CycleScenes", flags = ConVarFlags.None, helpText = "")]
        public static void CMD_CycleScenes(ConCommandArgs args)
        {
            return;
            new GameObject().AddComponent<LAI_SceneCycler>();
        }

        public class LAI_SceneCycler : MonoBehaviour
        {
            public List<string> sceneNames = new List<string>();
            public float durationPerScene = 3f;
            private float stopwatch = 0;
            public int index = 0;

            private bool hasTakenScreenshot = false;
            private float delayBeforeScreenshot = 2f;

            public void Awake()
            {
                sceneNames = LAISceneManager.scenesDict.Keys.ToList();
                SelectScene(sceneNames[index]);
            }

            public void Update()
            {
                stopwatch += Time.deltaTime;
                if (stopwatch > delayBeforeScreenshot)
                {
                    if (!hasTakenScreenshot)
                    {
                        hasTakenScreenshot = true;
                        ScreenCapture.CaptureScreenshot($"C:/Users/destr/Pictures/0Screenshots/{sceneNames[index]}.png");
                    }
                }
                if (stopwatch < durationPerScene)
                    return;
                stopwatch = 0;
                index++;
                hasTakenScreenshot = false;
                try
                {
                    SelectScene(sceneNames[index]);
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    new GameObject().AddComponent<LAI_LayoutCycler>();
                    enabled = false;
                    Destroy(this.gameObject);
                }
            }
        }

        public class LAI_LayoutCycler : MonoBehaviour
        {
            public List<string> layoutNames = new List<string>();
            public float durationPerScene = 3f;
            private float stopwatch = 0;
            public int index = 0;

            private bool hasTakenScreenshot = false;
            private float delayBeforeScreenshot = 2f;

            public void Awake()
            {
                layoutNames = LAILayoutManager.layoutsDict.Keys.ToList();
                layoutNames.Sort();
                while (layoutNames[index].StartsWith("any_"))
                {
                    index++;
                }
                Methods.LoadSceneAndLayout(GetSceneNameFromLayout(layoutNames[index]), layoutNames[index]);
                SelectScene(layoutNames[index]);
            }

            public string GetSceneNameFromLayout(string layoutName)
            {
                var holder = layoutName.Split('_');
                return holder[0];
            }

            public void Update()
            {
                stopwatch += Time.deltaTime;
                if (stopwatch > delayBeforeScreenshot)
                {
                    if (!hasTakenScreenshot)
                    {
                        hasTakenScreenshot = true;
                        ScreenCapture.CaptureScreenshot($"C:/Users/destr/Pictures/0Screenshots/{layoutNames[index]}.png");
                    }
                }
                if (stopwatch < durationPerScene)
                    return;
                stopwatch = 0;
                index++;
                hasTakenScreenshot = false;
                try
                {
                    Methods.LoadSceneAndLayout(GetSceneNameFromLayout(layoutNames[index]), layoutNames[index]);
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    enabled = false;
                    Destroy(this.gameObject);
                }
            }
        }
    }
}