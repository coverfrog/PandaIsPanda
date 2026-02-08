using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PandaIsPanda
{
    public class SceneRoot : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Bootstrap.IsBoot);
            
            Setup();
        }

        protected virtual void Setup() {}
    }
}