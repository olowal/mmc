using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MessageSystemObject : MonoBehaviour
{
    private void Update()
    {
        EventManager.SendEvents();
    }
}

public abstract class IMessageSystemHolder
{
    public abstract void Notify();
}

public class MessageSystemHolder<T> : IMessageSystemHolder
{
    //static readonly int size = 16;        Is this used?
    public delegate void CallEvent(T evt);

    public override void Notify()
    {
        int i, count = m_events.Count;
        for(i = 0; i < count; ++i)
        {
            for(LinkedListNode<Pair<MonoBehaviour,Action<T>>> it = m_listeners.First; it != null; it = it.Next)
            {
                it.Value.Second(m_events[i]);
            }
        }
        m_events.Clear();
    }

    public void Register(MonoBehaviour uid, Action<T> func)
    {
        m_listeners.AddFirst(new Pair<MonoBehaviour, Action<T>>(uid, func));
    }

    public void Remove(MonoBehaviour uid, Action<T> func)
    {
        for (LinkedListNode<Pair<MonoBehaviour, Action<T>>> it = m_listeners.First; it != null; it = it.Next)
        {
            if(it.Value.First == uid && it.Value.Second.Target == func.Target)
            {
                m_listeners.Remove(it);
                break;
            }
        }
    }

    public void Send(T evt)
    {
        m_events.Add(evt);
    }

    private LinkedList<Pair<MonoBehaviour, Action<T> >> m_listeners = new LinkedList<Pair<MonoBehaviour, Action<T>>>();
    private List<T> m_events = new List<T>();
}

public static class MessageSystem<T>
{
    static MessageSystem()
    {
        MessageSystemHolder<T> eventSystem = new MessageSystemHolder<T>();
        ms_eventSystem = eventSystem;
        EventManager.Add(eventSystem);
        Debug.Log("[MessageSystem()] Called constructor");
    }

    public delegate void CallEvent(ref T param);
    public static void Register(MonoBehaviour uid, Action<T> func)
    {
        ms_eventSystem.Register(uid, func);
    }

    public static void Remove(MonoBehaviour uid, Action<T> func)
    {
        ms_eventSystem.Remove(uid, func);
    }

    public static void Send(T evt)
    {
        ms_eventSystem.Send(evt);
    }

    private static MessageSystemHolder<T> ms_eventSystem = null;
}

public static class EventManager
{
    public static void Add(IMessageSystemHolder system)
    {
        ms_eventSystems.Add(system);
    }

    public static void SendEvents()
    {
        int i, count = ms_eventSystems.Count;
        for(i = 0; i < count; ++i)
        {
            ms_eventSystems[i].Notify();
        }
    }

    private static List<IMessageSystemHolder> ms_eventSystems = new List<IMessageSystemHolder>();
}
