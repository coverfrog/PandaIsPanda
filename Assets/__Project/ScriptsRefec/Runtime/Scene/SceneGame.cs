using System;
using UnityEngine;
using UnityEngine.Events;

namespace PandaIsPanda
{
    public class SceneGame : SceneRoot
    {
        [Header("# References")]
        [SerializeField] private GameStory m_gameStory;

        protected override void Setup()
        {
            base.Setup();
            
            LogUtil.Log($"[{nameof(SceneGame)}] Setup");

            IGame game = m_gameStory;
            game.Setup();
        }
    }
}