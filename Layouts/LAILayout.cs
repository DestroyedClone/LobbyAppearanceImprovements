﻿using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.Methods;

namespace LobbyAppearanceImprovements.Layouts
{
    public abstract class LAILayout
    {
        public LAILayout()
        {
        }

        // Technical Name of the scene
        public abstract string SceneLayout { get; }

        // Name of the Scene : "Lobby"
        public abstract string SceneName { get; }

        // Author of the scene : "DestroyedClone"
        public abstract string Author { get; }

        public virtual string LayoutNameToken { get; }

        public string LayoutTitleToken
        {
            get
            {
                return LayoutNameToken.IsNullOrWhiteSpace() ? string.Empty : LayoutNameToken + "_TITLE";
            }
        }

        public virtual string ReadmeDescription { get; }

        public virtual string[] RequiredModGUIDs { get; }

        public static Action<LAILayout> onLayoutLoaded;
        public static Action<LAILayout> onLayoutUnloaded;

        public abstract Dictionary<string, Vector3[]> CharacterLayouts { get; }

        public bool HasSetup = false;

        public virtual Dictionary<string, CameraSetting> CharacterCameraSettings { get; }

        public struct CameraSetting
        {
            public CameraSetting(float Fov = 60, Vector3 Position = new Vector3(), Vector3 Rotation = new Vector3())
            {
                fov = Fov;
                rotation = Rotation;
                position = Position;
            }

            public float fov { get; }
            public Vector3 position { get; }
            public Vector3 rotation { get; }
        }

        public bool CanLoadLayout()
        {
            if (RequiredModGUIDs != null && RequiredModGUIDs.Length > 0)
            {
                foreach (var GUID in RequiredModGUIDs) //Todo: Add optional assembly: "a.b.c||a.b.d"
                {
                    if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID))
                    {
                        LAILogging.LogMessage($"Refused to load layout \"{SceneLayout}\" because GUID \"{GUID}\" was not loaded!", LoggingStyle.UserShouldSee);
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsLayoutOfType<T>()
        {
            return this is T;
        }

        public GameObject CreateLayout()
        {
            var layoutHolder = new GameObject();
            layoutHolder.name = "HOLDER: LAYOUT (" + SceneLayout + ")";
            foreach (var characterLayout in CharacterLayouts)
            {
                CreateDisplay(characterLayout.Key, characterLayout.Value[0], characterLayout.Value[1], layoutHolder.transform);
            }
            foreach (var obj in CreateAdditionalObjectsOnLoad())
            {
                obj.transform.parent = layoutHolder.transform;
            }
            onLayoutLoaded?.Invoke(this);
            return layoutHolder;
        }

        public virtual List<GameObject> CreateAdditionalObjectsOnLoad()
        {
            return new List<GameObject>();
        }

        // For creating objects at runtime
        public virtual void Init()
        {
            LAILogging.LogMessage($"{SceneLayout}.Init :: Setting up layout.", LoggingStyle.UserShouldSee);
            if (HasSetup)
            {
                LAILogging.LogMessage($"{SceneLayout}.Init :: Ran Init(), but has already set up!", LoggingStyle.UserShouldSee);
                return;
            }
            HasSetup = true;
        }

        public void OnDestroy()
        {
            onLayoutUnloaded?.Invoke(this);
        }

        public class LAI_CharacterDisplay : MonoBehaviour
        {
            public string bodyName;
            public SurvivorDef survivorDef;

            public static Dictionary<SurvivorDef, GameObject> map = new Dictionary<SurvivorDef, GameObject>();
            //public static Dictionary<SurvivorDef, GameObject> mapBodyNames = new Dictionary<SurvivorDef, GameObject>();

            public void Awake()
            {
                InstanceTracker.Add(this);
                //map[survivorDef] = gameObject;
            }

            public void OnDestroy()
            {
                InstanceTracker.Remove(this);
                //map[survivorDef] = null;
            }

            public string nameOfThis = "";
            public string exposed = "";

            public void Update()
            {
                exposed = $"{{ \"{gameObject.name}\", " +
                    $"new Vector3[] {{" +
                    $"new Vector3({transform.localPosition.x}f, {transform.localPosition.y}f, {transform.localPosition.z}f), " +
                    $"new Vector3({transform.localRotation.eulerAngles.x}f, {transform.localRotation.eulerAngles.y}f, {transform.localRotation.eulerAngles.z}f)}} }},";
            }
        }
    }
}