using LeTai.Asset.TranslucentImage;
using RoR2;
using System;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.LAIPlugin;
using static LobbyAppearanceImprovements.Methods;

namespace LobbyAppearanceImprovements
{
    public static class Commands
    {
        [ConCommand(commandName = "LAI_SpawnPrefab", flags = ConVarFlags.ExecuteOnServer, helpText = "path x y z")]
        public static void CMD_SpawnPrefab(ConCommandArgs args)
        {
            var path = args.GetArgString(0);
            var gay = Resources.Load(path);
            var diorama = (GameObject)UnityEngine.Object.Instantiate(gay);
            diorama.transform.position = new Vector3(args.GetArgFloat(1), args.GetArgFloat(2), args.GetArgFloat(3));
        }

        [ConCommand(commandName = "LAI_ListScenes", flags = ConVarFlags.ExecuteOnServer, helpText = "Lists the available scenes.")]
        public static void CMD_ListScene(ConCommandArgs args)
        {
            foreach (var keyValuePair in scenesDict)
            {
                Debug.Log(keyValuePair.Key + " : " + keyValuePair.Value);
            }
        }

        [ConCommand(commandName = "LAI_SetScene", flags = ConVarFlags.ExecuteOnServer, helpText = "Sets the current scene to the specified name. | For previewing, does not save.")]
        public static void CMD_SetScene(ConCommandArgs args)
        {
            SelectScene(args.GetArgString(0));
        }

        [ConCommand(commandName = "LAI_ListLayouts", flags = ConVarFlags.ExecuteOnServer, helpText = "lai_listlayouts - shows all layouts." +
            "\n lai_listlayouts {sceneName} - shows all layouts for a particular scene")]
        public static void CMD_ListLayouts(ConCommandArgs args)
        {
            if (args.Count == 1)
            {
                var sceneName = args.GetArgString(0);
                foreach (var keyValuePair in layoutsDict)
                {
                    var instance = (CharacterSceneLayouts.CharSceneLayout)Activator.CreateInstance(keyValuePair.Value);
                    if (instance.SceneName == sceneName)
                        Debug.Log(keyValuePair.Key + " : " + keyValuePair.Value);
                }
            }
            foreach (var keyValuePair in layoutsDict)
            {
                Debug.Log(keyValuePair.Key + " : " + keyValuePair.Value);
            }
        }

        [ConCommand(commandName = "LAI_SetLayout", flags = ConVarFlags.ExecuteOnServer, helpText = "Sets the current layout to the specified name. | For previewing, does not save.")]
        public static void CMD_SetLayout(ConCommandArgs args)
        {
            SelectLayout(args.GetArgString(0));
        }

        [ConCommand(commandName = "LAI_ChangeLobbyColor", flags = ConVarFlags.ExecuteOnServer, helpText = "LAI_ChangeLobbyColor {r} {g} {b} {a} | For previewing, does not save.")]
        public static void ChangeLight(ConCommandArgs args)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "lobby")
            {
                Methods.ChangeLobbyLightColor(new Color32((byte)args.GetArgInt(0), (byte)args.GetArgInt(1), (byte)args.GetArgInt(2), (byte)args.GetArgInt(3)));
            }
        }

        [ConCommand(commandName = "LAI_getpos", flags = ConVarFlags.ExecuteOnServer, helpText = "lai_getpos | Returns bodyname, pos, rotation, in format for scenelayout file")]
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

        [ConCommand(commandName = "LAI_FadeOpacity", flags = ConVarFlags.ExecuteOnServer, helpText = "LAI_FadeOpacity {0-255} | For previewing, does not save.")]
        public static void CMD_adjustfade(ConCommandArgs args)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "lobby")
            {
                var ui_origin = GameObject.Find("CharacterSelectUI").transform;
                var SafeArea = ui_origin.Find("SafeArea").transform;
                var ui_left = SafeArea.Find("LeftHandPanel (Layer: Main)");
                var ui_right = SafeArea.Find("RightHandPanel");

                var shit = args.GetArgInt(0);

                var leftBlurColor = ui_left.Find("BlurPanel").GetComponent<TranslucentImage>();
                leftBlurColor.color = new Color(leftBlurColor.color.r,
                    leftBlurColor.color.g,
                    leftBlurColor.color.b,
                    Mathf.Clamp(shit, 0f, 255f));
                var rightBlurColor = ui_right.Find("RuleVerticalLayout").Find("BlurPanel").GetComponent<TranslucentImage>();
                rightBlurColor.color = new Color(leftBlurColor.color.r,
                    rightBlurColor.color.g,
                    rightBlurColor.color.b,
                    Mathf.Clamp(shit, 0f, 255f));
            }
        }
    }
}