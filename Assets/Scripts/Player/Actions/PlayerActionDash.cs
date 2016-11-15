using UnityEngine;
using System.Collections;

public class PlayerActionDash : PlayerAction
{
    [SerializeField]
    private float m_dashDistance = 2.0f;
    [SerializeField]
    private float m_speedMultiplier = 1.5f;

    private CharacterController2D m_controller = null;
    private Rigidbody2D m_body = null;
    private UID m_uid = null;
    private float m_startX = 0.0f;
    private float m_startDir = 0.0f;
    protected override void Awake()
    {
        base.Awake();
        m_action = Actions.Dash;
        InputController inputController = transform.GetComponentFromParentRecursive<InputController>(true);
        if(inputController != null)
        {
            InputController.ButtonDownHandler dashPressed = OnDashPressed;
            InputController.ButtonUpHandler dashReleased = OnDashReleased;
            InputController.AxisHandler axis = OnAxis;
            inputController.RegisterHandler("Dash", this, dashPressed);
            inputController.RegisterHandler("Dash", this, dashReleased);
            inputController.RegisterHandler("Horizontal", this, axis);
        }
        m_controller = transform.GetComponentFromParentRecursive<CharacterController2D>(true);
        m_body = transform.GetComponentFromParentRecursive<Rigidbody2D>(true);
        m_uid = transform.GetComponentFromParentRecursive<UID>(true);
        enabled = false;
        MessageSystem<OnEnterAirEvent>.Register(this, OnEnterAir);
    }

    void OnDestroy()
    {
        MessageSystem<OnEnterAirEvent>.Remove(this, OnEnterAir);
    }

    void FixedUpdate()
    {
        if(m_controller.grounded)
        {
            m_controller.movement = m_controller.facingDirection;
            float diff = Mathf.Abs(m_body.position.x - m_startX);
            if (diff >= m_dashDistance)
            {
                DeactivateAction();
            }
        }
    }

    void OnDashPressed()
    {
        if(m_controller.grounded)
        {
            ActivateAction();
        }
    }

    void OnDashReleased()
    {
        if(m_controller.grounded)
        {
            DeactivateAction();
        }
    }

    void OnEnterAir(OnEnterAirEvent evt)
    {
        if(enabled && evt.uid == m_uid.value)
        {
            m_controller.movement = 0.0f;
        }
    }

    void OnExitAir(OnExitAirEvent evt)
    {
        if(enabled && evt.uid == m_uid.value)
        {
            DeactivateAction();
        }
    }

    public override void OnEnterAction()
    {
        enabled = true;
        m_startX = m_body.position.x;
        m_startDir = m_controller.facingDirection;
        m_controller.speedMultiplier = m_speedMultiplier;
    }

    public override void OnExitAction()
    {
        m_controller.speedMultiplier = 1.0f;
        enabled = false;
        MessageSystem<OnExitDash>.Send(new OnExitDash(m_uid.value));
    }

    void OnAxis(float value)
    {
        if(enabled && value != 0.0f && value != m_startDir)
        {
            DeactivateAction();
        }
    }
}
