using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using static LobbyAppearanceImprovements.LAIPlugin;

namespace LobbyAppearanceImprovements
{
    public static class Commands
    {
        [ConCommand(commandName = "LAI_SpawnPrefab", flags = ConVarFlags.ExecuteOnServer, helpText = "stagename x y z")]
        public static void CMD_SpawnPrefab(ConCommandArgs args)
        {
            var path = args.GetArgString(0);
            var gay = Resources.Load(path);
            var diorama = (GameObject)UnityEngine.Object.Instantiate(gay);
            diorama.transform.position = new Vector3(args.GetArgFloat(1), args.GetArgFloat(2), args.GetArgFloat(3));
        }

        [ConCommand(commandName = "LAI_ListScene", flags = ConVarFlags.ExecuteOnServer, helpText = "stagename x y z")]
        public static void CMD_ListScene(ConCommandArgs args)
        {
            foreach (var keyValuePair in scenesDict)
            {
                Debug.Log("Key = {0},Value = {1}" + keyValuePair.Key + keyValuePair.Value);
            }
        }

        [ConCommand(commandName = "LAI_SetScene", flags = ConVarFlags.ExecuteOnServer, helpText = "stagename x y z")]
        public static void CMD_SetScene(ConCommandArgs args)
        {
            //SelectScene(scenesDict[args.GetArgString(0)]);
        }
    }
}
