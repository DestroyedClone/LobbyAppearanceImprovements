using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using R2API;
using RoR2.UI;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace LobbyAppearanceImprovements
{
    public static class LAIAssets
    {
        public static async Task Init()
        {
            static async Task Test()
            {
                var task1 = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/HGCreditNameLabel.prefab");
                var task2 = Addressables.LoadAssetAsync<TMP_FontAsset>("RoR2/Base/Common/Fonts/Bombardier/tmpBombDropshadow.asset");
                var task3 = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/Fonts/Bombardier/tmpBombPlain.mat");

                bombardierTextObject = (await task1.Task).InstantiateClone("LAI_BombardierTextObject", false);
                var textmesh = bombardierTextObject.GetComponent<HGTextMeshProUGUI>();
                textmesh.font = await task2.Task;
                textmesh.material = await task3.Task;
                textmesh.enableKerning = true;
                textmesh.enableAutoSizing = true;
                textmesh.fontSize = 38.5f; //24

                UnityEngine.Object.Destroy(bombardierTextObject.GetComponent<ContentSizeFitter>());
            }

            await Test().ConfigureAwait(false);
        }

        //public static GameObject genericTextObject;
        public static GameObject bombardierTextObject;
        //private static TMP_FontAsset bombardierFontAsset;
        //private static Material bombardierMaterial;
    }
}
