using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using RoR2;
using R2API;
using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using EntityStates;
using HG;
using RoR2.Audio;
using RoR2.Navigation;
using RoR2.Networking;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2.UI;
using Unity;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace LobbyAppearanceImprovements.Scenes
{
    public abstract class LAIScene
    {
        public abstract string SceneName { get; }
        public abstract GameObject BackgroundPrefab { get; }
        public abstract Vector3 Position { get; }
        public abstract Vector3 Rotation { get; }
    }
}
