using UnityEngine;
using System.Collections;

// Only used for movement on the ground
public class PlayerActionMove : PlayerAction
{
    private CharacterController2D m_controller = null;

    private float m_right = 0.0f;
    private float m_left = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        m_action = Actions.Move;
        InputController inputController = transform.GetComponentFromParentRecursive<InputController>(true);
        if(inputController != null)
        {
            InputController.ButtonDownHandler moveRightPressed = OnMoveRightPressed;
            InputController.ButtonUpHandler moveRightReleased = OnMoveRightReleased;
            InputController.ButtonDownHandler moveLeftPressed = OnMoveLeftPressed;
            InputController.ButtonUpHandler moveLeftReleased = OnMoveLeftReleased;

            inputController.RegisterHandler("MoveRight", this, moveRightPressed);
            inputController.RegisterHandler("MoveRight", this, moveRightReleased);
            inputController.RegisterHandler("MoveLeft", this, moveLeftPressed);
            inputController.RegisterHandler("MoveLeft", this, moveLeftReleased);
        }
        m_controller = transform.GetComponentFromParentRecursive<CharacterController2D>(true);
    }

    void OnAxis(float value)
    {
        ActionManager.Results results = ActivateAction();
        if(results == ActionManager.Results.Running || results == ActionManager.Results.Success)
        {
            m_controller.movement = value;
        }
        if(value == 0.0f)
        {
            DeactivateAction();
        }
    }

    void OnMoveLeftPressed()
    {
        ActionManager.Results results = ActivateAction();
        if(results == ActionManager.Results.Success || results == ActionManager.Results.Running)
        {
            m_left = -1.0f;
            SetMovementDirection();
        }
    }

    void OnMoveLeftReleased()
    {
        if(enabled)
        {
            m_left = 0.0f;
            SetMovementDirection();
        }
    }

    void OnMoveRightPressed()
    {
        m_right = 1.0f;
        SetMovementDirection();
    }

    void OnMoveRightReleased()
    {
        if(enabled)
        {
            m_right = 0.0f;
            SetMovementDirection();
        }
    }

    void SetMovementDirection()
    {
        float dir = m_left + m_right;
        m_controller.movement = dir;
        if(dir == 0.0f)
        {
            DeactivateAction();
        }
    }

    public override void OnEnterAction()
    {
        enabled = true;
    }

    public override void OnExitAction()
    {
        enabled = false;
    }
}
