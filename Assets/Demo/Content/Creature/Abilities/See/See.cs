﻿// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DanielLochner.Assets.CreatureCreator
{
    [CreateAssetMenu(menuName = "Creature Creator/Ability/See")]
    public class See : Ability
    {
        [Header("See")]
        [SerializeField] private PostProcessProfile defaultProfile;
        [SerializeField] private PostProcessProfile blindedProfile;

        public override void OnAdd()
        {
            PostProcessManager.Instance.BlendToProfile(defaultProfile, 0.25f);
        }
        public override void OnRemove()
        {
            PostProcessManager.Instance.BlendToProfile(blindedProfile, 0.25f);
        }
    }
}