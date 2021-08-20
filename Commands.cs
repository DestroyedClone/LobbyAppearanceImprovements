using RoR2;
using System;
using UnityEngine;
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
                    var instance = (CharacterSceneSetups.CharSceneLayout)Activator.CreateInstance(keyValuePair.Value);
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
    }
}