using UnityEngine;
using System.Collections.Generic;

public class InputController : MonoBehaviour
{
    private List<string> m_axis = new List<string>();
    private List<string> m_buttons = new List<string>();
    public delegate void AxisHandler(float value);
    public delegate void ButtonDownHandler();
    public delegate void ButtonUpHandler();
    Dictionary<string, List<Pair<MonoBehaviour, AxisHandler>>> m_axisHandlers = new Dictionary<string, List<Pair<MonoBehaviour, AxisHandler>>>();
    Dictionary<string, List<Pair<MonoBehaviour, ButtonDownHandler>>> m_buttonDownHandlers = new Dictionary<string, List<Pair<MonoBehaviour, ButtonDownHandler>>>();
    Dictionary<string, List<Pair<MonoBehaviour, ButtonUpHandler>>> m_buttonUpHandlers = new Dictionary<string, List<Pair<MonoBehaviour, ButtonUpHandler>>>();

    public bool RegisterHandler(string axis, MonoBehaviour uid, AxisHandler handler)
    {
        if(!m_axis.Contains(axis))
        {
            m_axis.Add(axis);
        }
        List<Pair<MonoBehaviour, AxisHandler>> handlers = null;
        if (!m_axisHandlers.ContainsKey(axis))
        {
            handlers = new List<Pair<MonoBehaviour, AxisHandler>>();
            handlers.Add(new Pair<MonoBehaviour,AxisHandler>(uid, handler));
            m_axisHandlers.Add(axis, handlers);
            Debug.Log("[InputController:RegisterHandler] AxisHandler " + axis);
        }
        else
        {
            if(m_axisHandlers.TryGetValue(axis, out handlers))
            {
                int i, count = handlers.Count;
                for(i = 0; i < count; ++i)
                {
                    if(handlers[i].First == uid)
                    {
                        return false;
                    }
                }

                handlers.Add(new Pair<MonoBehaviour, AxisHandler>(uid, handler));
                Debug.Log("[InputController:RegisterHandler] AxisHandler " + axis);
            }
        }
        return true;
    }

    public bool RegisterHandler(string button, MonoBehaviour uid, ButtonDownHandler handler)
    {
        if(!m_buttons.Contains(button))
        {
            m_buttons.Add(button);
        }
        List<Pair<MonoBehaviour, ButtonDownHandler>> handlers = null;
        if (!m_buttonDownHandlers.ContainsKey(button))
        {
            handlers = new List<Pair<MonoBehaviour, ButtonDownHandler>>();
            handlers.Add(new Pair<MonoBehaviour, ButtonDownHandler>(uid, handler));
            m_buttonDownHandlers.Add(button, handlers);
            Debug.Log("[InputController:RegisterHandler] ButtonDownHandler " + button + " was registered");
        }
        else
        {
            if (m_buttonDownHandlers.TryGetValue(button, out handlers))
            {
                int i, count = handlers.Count;
                for (i = 0; i < count; ++i)
                {
                    if (handlers[i].First == uid)
                    {
                        return false;
                    }
                }

                handlers.Add(new Pair<MonoBehaviour, ButtonDownHandler>(uid, handler));
                Debug.Log("[InputController:RegisterHandler] ButtonDownHandler " + button + " was registered");
            }
        }
        return true;
    }

    public bool RegisterHandler(string button, MonoBehaviour uid, ButtonUpHandler handler)
    {
        if (!m_buttons.Contains(button))
        {
            m_buttons.Add(button);
        }
        List<Pair<MonoBehaviour, ButtonUpHandler>> handlers = null;
        if (!m_buttonUpHandlers.ContainsKey(button))
        {
            handlers = new List<Pair<MonoBehaviour, ButtonUpHandler>>();
            handlers.Add(new Pair<MonoBehaviour, ButtonUpHandler>(uid, handler));
            m_buttonUpHandlers.Add(button, handlers);
            Debug.Log("[InputController:RegisterHandler] ButtonUpHandler " + button + " was registered");
        }
        else
        {
            if (m_buttonUpHandlers.TryGetValue(button, out handlers))
            {
                int i, count = handlers.Count;
                for (i = 0; i < count; ++i)
                {
                    if (handlers[i].First == uid)
                    {
                        return false;
                    }
                }

                handlers.Add(new Pair<MonoBehaviour, ButtonUpHandler>(uid, handler));
                Debug.Log("[InputController:RegisterHandler] ButtonUpHandler " + button + " was registered");
            }
        }
        return true;
    }

    void Update()
    {
        int i, j, count, len = m_axis.Count;
        for(i = 0; i < len; ++i)
        {
            string axis = m_axis[i];
            float value = Input.GetAxisRaw(axis);

            List<Pair<MonoBehaviour, AxisHandler>> handlers;
            if (m_axisHandlers.TryGetValue(axis, out handlers))
            {
                
                count = handlers.Count;
                for(j = 0; j < count; ++j)
                {
                    handlers[j].Second(value);
                }
            }
        }

        len = m_buttons.Count;
        for(i = 0; i < len; ++i)
        {
            string button = m_buttons[i];
            bool down = Input.GetButtonDown(button);
            bool up = Input.GetButtonUp(button);
            if(down)
            {
                List<Pair<MonoBehaviour, ButtonDownHandler>> handlers;
                if(m_buttonDownHandlers.TryGetValue(button, out handlers))
                {
                    count = handlers.Count;
                    for(j = 0; j < count; ++j)
                    {
                        handlers[j].Second();
                    }
                }
            }
            if(up)
            {
                List<Pair<MonoBehaviour, ButtonUpHandler>> handlers;
                if (m_buttonUpHandlers.TryGetValue(button, out handlers))
                {
                    count = handlers.Count;
                    for (j = 0; j < count; ++j)
                    {
                        handlers[j].Second();
                    }
                }
            }
        }
    }
}
