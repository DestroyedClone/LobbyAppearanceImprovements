using RoR2;
using UnityEngine;

namespace LobbyAppearanceImprovements
{
    public partial class LAIPlugin
    {
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