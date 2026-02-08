using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace PandaIsPanda
{
    public static class Bootstrap
    {
        public static bool IsBoot { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Boot()
        {
            BootTask().Forget();
        }

        private static async UniTask BootTask()
        {
            await UniTask.WhenAll
            (
                AddressableUtil.InstantiateAsync<UIManager>("manager/ui", true),
                AddressableUtil.InstantiateAsync<AudioManager>("manager/audio", true)
            );
            
            IsBoot = true;
        }
    }
}
