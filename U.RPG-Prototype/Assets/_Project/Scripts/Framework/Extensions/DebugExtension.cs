/*
 * DebugExtension - Custom functionality for Unity Debug
 * Created by : Allan N. Murillo
 * Last Edited : 8/12/2021
 */

using System;
using UnityEngine;

namespace ANM.Framework.Extensions
{
    public static class DebugExtension
    {
        public static void Log(string msg, Type scriptType)
        {
            Debug.Log($"[{scriptType.Name}]: " + msg);
        }

        public static void LogWarning(string msg, Type scriptType)
        {
            Debug.LogWarning($"[{scriptType.Name}]: " + msg);
        }

        public static void LogError(string msg, Type scriptType)
        {
            Debug.LogError($"[{scriptType.Name}]: " + msg);
        }
    }
}
