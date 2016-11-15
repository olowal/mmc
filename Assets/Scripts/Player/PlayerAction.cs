using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    protected Actions m_action;
    public Actions action { get { return m_action; } }
    private ActionManager m_actionManager = null;
    protected ActionManager actionManager { get { return m_actionManager; } }

    [SerializeField]
    private Actions[] m_blocks;
    [SerializeField]
    private Actions[] m_interruptedBy;

    public enum Actions
    {
        Move,
        MoveAir,
        Jump,
        Dash,
        Hurt,
        Shoot,
        Ground_Slice,
        Air_Slice,
        All,
        None,
    };

    protected virtual void Awake()
    {
        m_actionManager = transform.GetComponentFromParentRecursive<ActionManager>(true);
    }

    protected ActionManager.Results ActivateAction()
    {
        return actionManager.ActivateAction(this);
    }

    protected ActionManager.Results DeactivateAction()
    {
        return actionManager.DeactivateAction(this);
    }

    protected virtual void OnInterrupt()
    {

    }

    public bool IsBlocking(Actions action)
    {
        int i, len = m_blocks.Length;
        for(i = 0; i < len; ++i)
        {
            if(action == m_blocks[i])
            {
                return true;
            }
        }
        return false;
    }

    public bool Interrupt(Actions action)
    {
        int i, len = m_interruptedBy.Length;
        for(i = 0; i < len; ++i)
        {
            if (action == m_interruptedBy[i])
            {
                if(CanBeInterrupted(action))
                {
                    OnInterrupt();
                    return true;
                }
            }
        }
        return false;
    }

    public virtual void OnEnterAction()
    {

    }

    public virtual void OnExitAction()
    {

    }

    public virtual bool CanBeInterrupted(PlayerAction.Actions action)
    {
        return true;
    }
}
