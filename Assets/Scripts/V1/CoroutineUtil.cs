using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtil
{
    public static IEnumerator DelayAction(System.Action codeToRun, object delay)
    {
        yield return delay;
        codeToRun();
    }
}
