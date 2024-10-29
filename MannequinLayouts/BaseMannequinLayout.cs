using System.Collections.Generic;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;

namespace LobbyAppearanceImprovements.MannequinLayouts
{
    public abstract class BaseMannequinLayout
    {
        public BaseMannequinLayout()
        {
        }

        public abstract string NameToken { get; }
        public abstract string InternalName { get; }
        public virtual string Credit { get; }

        public abstract List<TransformInfo> MannequinTransforms { get; }

        public struct TransformInfo
        {
            public TransformInfo(Vector3 position, Vector3 rotation, Vector3 scale)
            {
                this.position = position;
                this.rotation = rotation;
                this.scale = scale;
            }

            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
        }

        public bool HasSetup = false;

        public virtual void Init()
        {
            var nameOfThis = GetType().Name;
            LAILogging.LogMessage($"{nameOfThis}.Init :: Setting up mannequin layout.", LoggingStyle.ObscureSoOnlyDevSees);
            if (HasSetup)
            {
                LAILogging.LogMessage($"{nameOfThis}.Init :: Ran Init(), but has already set up!", LoggingStyle.ObscureSoOnlyDevSees);
                return;
            }
            HasSetup = true;
        }

        public void CreateMannequinLayout()
        {
        }

        public bool IsOfType<T>()
        {
            return this is T;
        }

        public virtual void ModifyMannequin(GameObject mannequinGameObject)
        {
            var go = mannequinGameObject;
        }
    }
}