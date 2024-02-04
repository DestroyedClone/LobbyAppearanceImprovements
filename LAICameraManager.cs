using LobbyAppearanceImprovements.CharacterSceneLayouts;
using System.Collections.Generic;
using static LobbyAppearanceImprovements.LAIPlugin;
using RoR2;
using RoR2.CameraModes;

namespace LobbyAppearanceImprovements
{
    internal static class LAICameraManager
    {
        public static Dictionary<string, CharSceneLayout.CameraSetting> currentCameraSettings = new Dictionary<string, CharSceneLayout.CameraSetting>();

        //public static GameObject DefaultTextObject;

        public static Methods.LAICameraController CurrentCameraController;

        public static void Init()
        {
            On.RoR2.CameraRigController.Start += CameraRigController_Start;
        }

        public static void CameraRigController_Start(On.RoR2.CameraRigController.orig_Start orig, CameraRigController self)
        {
            orig(self);
            var a = self.gameObject.AddComponent<CameraController>();
            a.SetCam(self);
            a.enabled = false;
        }
    }
}