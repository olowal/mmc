using UnityEngine;

public static class TransformEx
{
    // Gets a component recursively from parents
    public static T GetComponentFromParentRecursive<T>(this Transform t) where T : Component
    {
        Transform p = t.parent;
        T c = null;

        if (p != null)
        {
            c = p.GetComponent<T>();
            if(c == null)
            {
                c = p.GetComponentFromParentRecursive<T>();
            }
        }

        return c;
    }

    public static T GetComponentFromParentRecursive<T>(this Transform t, bool checkSelf) where T : Component
    {
        T c = t.GetComponent<T>();
        if(c == null)
        {
            return t.GetComponentFromParentRecursive<T>();
        }
        return c;
    }
}
