using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIScriptUtil
{
    public static void AddChildrenToList<T>(Transform parent, List<T> list, List<int> excludedIndexes = null) where T : MonoBehaviour
    {
        for (int i = 0; i < parent.childCount; i++)
            if (!(excludedIndexes?.Contains(i)??false) && parent.GetChild(i).TryGetComponent(out T component))
                list.Add(component);
    }
}
