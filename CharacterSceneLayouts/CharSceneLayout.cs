﻿using System;
using System.Collections.Generic;
using UnityEngine;
using static LobbyAppearanceImprovements.ConfigSetup;
using static LobbyAppearanceImprovements.Methods;

namespace LobbyAppearanceImprovements.CharacterSceneLayouts
{
    public abstract class CharSceneLayout
    {
        public CharSceneLayout()
        {
        }

        // Technical Name of the scene
        public abstract string SceneLayout { get; }

        // Name of the Scene : "Lobby"
        public abstract string SceneName { get; }

        // Author of the scene : "DestroyedClone"
        public abstract string Author { get; }

        // Name of the layout : "Original Crew"
        public abstract string LayoutName { get; }

        public virtual string ReadmeDescription { get; }

        public virtual string[] RequiredModGUIDs { get; }

        public abstract Dictionary<string, Vector3[]> CharacterLayouts { get; }

        public bool HasSetup = false;

        public virtual Dictionary<string, CameraSetting> CharacterCameraSettings { get; }

        public struct CameraSetting
        {
            public CameraSetting(float Fov = 60, float Pitch = 0, float Yaw = 0, Vector3 Position = new Vector3(), Vector3 Rotation = new Vector3())
            {
                fov = Fov;
                pitch = Pitch;
                yaw = Yaw;
                rotation = Rotation;
                position = Position;
            }

            public float fov { get; }
            public float pitch { get; }
            public float yaw { get; }
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
                        LAILogging.LogMessage($"Refused to load layout \"{LayoutName}\" because GUID \"{GUID}\" was not loaded!", LoggingStyle.UserShouldSee);
                        return false;
                    }
                }
            }
            return true;
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
    }
}