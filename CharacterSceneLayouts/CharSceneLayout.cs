﻿using UnityEngine;
using static LobbyAppearanceImprovements.Methods;
using System.Collections.Generic;

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

        public abstract Dictionary<string, Vector3[]> CharacterLayouts { get; }

        public bool HasSetup = false;

        public GameObject CreateLayout()
        {
            var layoutHolder = new GameObject();
            layoutHolder.name = "HOLDER: LAYOUT ("+ SceneLayout+")";
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
            return null;
        }

        // For creating objects at runtime
        public virtual void Init()
        {
            Debug.Log("Setting up layout for "+ SceneLayout);
            if (HasSetup)
            {
                Debug.Log("Layout already ran Init()!");
                return;
            }
            HasSetup = true;
        }
    }
}