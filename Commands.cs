using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;

namespace LobbyAppearanceImprovements
{
    public static class Commands
    {
        [ConCommand(commandName = "LAI_DioramaTest", flags = ConVarFlags.ExecuteOnServer, helpText = "stagename x y z")]
        public static void Diorama(ConCommandArgs args)
        {
            var path = "prefabs/stagedisplay/" + args.GetArgString(0) + "DioramaDisplay";
            var gay = Resources.Load(path);
            var diorama = (GameObject)UnityEngine.Object.Instantiate(gay);
            diorama.transform.position = new Vector3(args.GetArgFloat(1), args.GetArgFloat(2), args.GetArgFloat(3));
        }
    }
}
