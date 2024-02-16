using LobbyAppearanceImprovements.CharacterSceneLayouts;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

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

        public class CameraController : MonoBehaviour
        {
            public float fov = 60;
            public float pitch = 0;
            public float yaw = 0;
            public bool restart = false;
            public bool logit = false;
            private CameraRigController cam;

            public void SetCam(CameraRigController newCam)
            {
                cam = newCam;
            }

            public void FixedUpdate()
            {
                if (restart)
                {
                    fov = 60;
                    pitch = 0;
                    yaw = 0;
                    restart = false;
                    return;
                }
                cam.baseFov = fov;
                //camera rig controller gets cameramodecontext from itself
                //
                /*cam.GenerateCameraModeContext(out CameraModeBase.CameraModeContext cameraModeContext);
                object rawInstanceData = cam.cameraMode.camToRawInstanceData[cameraModeContext.cameraInfo.cameraRigController];
                ((CameraModePlayerBasic.InstanceData)rawInstanceData).pitchYaw = new PitchYawPair()
                {
                    pitch = pitch,
                    yaw = yaw
                };
                cam.cameraModeContext = cameraModeContext;*/
                //cam.pitch = pitch;
                //cam.yaw = yaw;
                if (logit)
                {
                    Debug.Log($"new CameraSetting( {fov}, {pitch}, {yaw} )");
                    logit = false;
                }
            }
        }
    }
}