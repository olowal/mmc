using UnityEngine;
using System.Collections;

public static class GameObjectEx
{
    public static GameObject GetParentRecursive(this GameObject gameObject, string name)
    {
        GameObject obj = null;
        Transform parent = gameObject.transform.parent;
        if(parent)
        {
            obj = gameObject.transform.parent.gameObject;
            if(string.Equals(obj.name, name))
            {
                return obj;
            }
            else
            {
                obj = obj.GetParentRecursive(name); 
            }
        }

        return obj;
    }

    public static GameObject GetParentByComponentRecursive<T>(this GameObject gameObject) where T : Component
    {
        GameObject obj = null;
        Transform parent = gameObject.transform.parent;

        if(parent != null)
        {
            T component = parent.GetComponent<T>();
            if(component != null)
            {
                return component.gameObject;
            }
            else
            {
                obj = parent.gameObject.GetParentByComponentRecursive<T>();
            }
        }

        return obj;
    }
}
