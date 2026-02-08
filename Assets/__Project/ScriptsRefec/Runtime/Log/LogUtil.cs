using UnityEngine;

namespace PandaIsPanda
{
    public static class LogUtil
    {
        public static void Log(object msg, bool isClientLog = false)
        {
#if UNITY_ASSERTIONS
            Debug.unityLogger.Log(LogType.Log, msg);
#endif
        }
    }
}