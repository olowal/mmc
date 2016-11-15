using UnityEngine;
using System.Collections.Generic;

//  Manage actions and what is allowed/not allowed to be
public class ActionManager : MonoBehaviour
{
    //  Return values when attempting to activate an action
    public enum Results
    {
        Success,    // If the action activation was successful
        Running,    // If the action is already running (when attempting to activate)
        NotRunning, // If the action is not running (when attempting to deactivate)
        Failure     // If the action failed to activate or deactivate
    }

    private LinkedList<PlayerAction> m_active = new LinkedList<PlayerAction>();
    private List<PlayerAction> m_interrupts = new List<PlayerAction>();

    private bool IsBlocked(PlayerAction.Actions action)
    {
        for(LinkedListNode<PlayerAction> it = m_active.First; it != null; it = it.Next)
        {
            if(it.Value.IsBlocking(action))
            {
                return true;
            }
        }
        return false;
    }

    private void Interrupt(PlayerAction.Actions action)
    {
        m_interrupts.Clear();
        for (LinkedListNode<PlayerAction> it = m_active.First; it != null; it = it.Next)
        {
            if(it.Value.Interrupt(action))
            {
                m_interrupts.Add(it.Value);
            }
        }

        int i, count = m_interrupts.Count;
        for(i = 0; i < count; ++i)
        {
            m_active.Remove(m_interrupts[i]);
        }
        m_interrupts.Clear();
    }

    public Results ActivateAction(PlayerAction action)
    {
        if(m_active.Contains(action))
        {
            return Results.Running;
        }

        if(IsBlocked(action.action))
        {
            return Results.Failure;
        }

        m_active.AddLast(action);
        action.OnEnterAction();
        return Results.Success;
    }

    public Results DeactivateAction(PlayerAction action)
    {
        LinkedListNode<PlayerAction> node = m_active.Find(action);
        if(node == null)
        {
            return Results.NotRunning;
        }

        if(node != null)
        {
            node.Value.OnExitAction();
            m_active.Remove(node);
            return Results.Success;
        }

        return Results.Failure;
    }
}
