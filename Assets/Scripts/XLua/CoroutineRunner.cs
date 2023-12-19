using UnityEngine;
using XLua;
using System.Collections.Generic;
using System.Collections;
using System;

/// <summary>
/// lua使用C#携程辅助类
/// </summary>
[Hotfix]
public class CoroutineRunner : MonoBehaviour
{
    public void YieldAndCallback(object toYield, Action callback)
    {
        StartCoroutine(CoBody(toYield, callback));
    }

    private IEnumerator CoBody(object toYield, Action callback)
    {
        if (toYield is IEnumerator)
            yield return StartCoroutine((IEnumerator)toYield);
        else
            yield return toYield;
        callback();
    }
}

#if UNITY_EDITOR
public static class CoroutineRunnerExporter
{
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>() {
                typeof(WaitForSeconds),
                typeof(WaitForEndOfFrame),
                typeof(WaitForFixedUpdate),
                typeof(WWW),
        };
}
#endif
