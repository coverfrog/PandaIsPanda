using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace PandaIsPanda
{
    public class SceneTitle : SceneRoot
    {
        protected override void Setup()
        {
            base.Setup();

            StartCoroutine(CoWait(1.0f, () =>
            {
                SceneManager.LoadScene("Lobby");
            }));
        }

        private IEnumerator CoWait(float seconds, Action callback)
        {
            for (float t = 0.0f; t < seconds; t += Time.deltaTime)
            {
                yield return null;
            }
            
            callback?.Invoke();
        }
    }
}