using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions {

    public static Transform FindBrotherWithTag(this Transform transform, params string[] tags)
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            var child = transform.parent.GetChild(i);
            for (int t = 0; t < tags.Length; t++) if (child.CompareTag(tags[t])) return child;
        }
        return null;
    }
    public static Transform FindChildWithTag(this Transform transform, params string[] tags)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            for (int t = 0; t < tags.Length; t++) if (child.CompareTag(tags[t])) return child;
        }
        return null;
    }
    public static int GetDepth(this Transform transform)
    {
        if (transform == null) return 0;
        int i = 0;
        var T = transform;
        while (T.parent != null)
        {
            i++;
            T = T.parent;
        }
        return i;
    }

}
