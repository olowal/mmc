using UnityEngine;
using UnityEngine.EventSystems;


public static class UIEx
{
    public static bool IsCursorOnUI()
    {
        if(EventSystem.current.IsPointerOverGameObject() || (Input.touchSupported && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
        {
            return true;
        }

        return false;
    }
}
