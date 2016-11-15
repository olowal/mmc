using UnityEngine;
using System.Collections;

public class PlayerActionJump : PlayerAction
{
    [SerializeField]
    private float m_maxJumpHeight = 3.0f;

    private CharacterController2D m_controller = null;
    private Rigidbody2D m_body = null;

    private float m_jumpStartY = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        m_action = Actions.Jump;
        InputController inputController = transform.GetComponentFromParentRecursive<InputController>(true);
        if(inputController != null)
        {
            InputController.ButtonDownHandler jumpPressed = OnJumpPressed;
            InputController.ButtonUpHandler jumpReleased = OnJumpReleased;
            inputController.RegisterHandler("Jump", this, jumpPressed);
            inputController.RegisterHandler("Jump", this, jumpReleased);
        }

        m_controller = transform.GetComponentFromParentRecursive<CharacterController2D>(true);
        m_body = transform.GetComponentFromParentRecursive<Rigidbody2D>(true);
        enabled = false;
    }


    void FixedUpdate()
    {
        if(m_controller.isJumping)
        {
            m_controller.Jump();
            if ((m_body.position.y - m_jumpStartY) >= m_maxJumpHeight)
            {
                DeactivateAction();
            }
        }
    }

    void OnJumpPressed()
    {
        if(m_controller.grounded)
        {
            ActivateAction();
        }
    }

    void OnJumpReleased()
    {
        DeactivateAction();
    }

    public override void OnEnterAction()
    {
        m_controller.Jump();
        m_jumpStartY = m_body.position.y;
        enabled = true;
    }

    public override void OnExitAction()
    {
        enabled = false;
    }
}
