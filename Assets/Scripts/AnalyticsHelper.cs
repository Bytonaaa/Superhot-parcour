using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public static class AnalyticsHelper
{
    // Ничего не переставлять, новые значения добавлять после имеющихся! Иначе поломаются данные статистики.
    public enum PlayerDeath { fallthrough, timeout, collision, bulletCollision }

    public static void LogSceneLoadEvent(string sceneName)
    {
        if (!Application.isEditor)
        {
            Analytics.CustomEvent("sceneLoad", new Dictionary<string, object>
            {
                { "sceneName", sceneName }
            });
        }
        else
        {
            Debug.Log("AnalyticsHelper.LogSceneLoadEvent(" + sceneName + ")");
        }
    }

    public static void LogSceneRestartEvent(string sceneName, PlayerDeath reason)
    {
        if (!Application.isEditor)
        {
            Analytics.CustomEvent("sceneRestart", new Dictionary<string, object>
            {
                { "sceneName", sceneName },
                { "restartReason", reason }
            });
        }
        else
        {
            Debug.Log("AnalyticsHelper.LogSceneRestartEvent(" + sceneName + ", " + reason + ")");
        }
    }
}
