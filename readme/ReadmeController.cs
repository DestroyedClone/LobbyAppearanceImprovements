using LobbyAppearanceImprovements.CharacterSceneLayouts;
using System;
using UnityEngine;

namespace LobbyAppearanceImprovements.readme
{
    public static class ReadmeController
    {
        //scene, layout, characters, note
        public const string thing = "| {0} | {1} | {2} | {3} ";

        public static void Output()
        {
            var sb = HG.StringBuilderPool.RentStringBuilder();
            foreach (var entry in LAILayoutManager.layoutsDict)
            {
                var characterStringBuilder = HG.StringBuilderPool.RentStringBuilder();
                var lay = (CharSceneLayout)Activator.CreateInstance(entry.Value);
                foreach (var chr in lay.CharacterLayouts)
                {
                    characterStringBuilder.Append($"{chr.Key}, ");
                }

                var sceneName = lay.RequiredModGUIDs.Length == 0 ? "🔒" : "";
                sceneName += lay.SceneName;

                sb.AppendLine(string.Format(thing, sceneName, lay.SceneLayout, characterStringBuilder.ToString(), lay.ReadmeDescription));
                HG.StringBuilderPool.ReturnStringBuilder(characterStringBuilder);
            }
            Debug.Log(sb.ToString());
            HG.StringBuilderPool.ReturnStringBuilder(sb);
        }
    }
}