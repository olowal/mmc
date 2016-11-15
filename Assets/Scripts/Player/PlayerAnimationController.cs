using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Rigidbody2D m_body = null;
    private Animator m_animator = null;
    private CharacterController2D m_controller = null;
    private UID m_uid = null;

    void Awake()
    {
        m_body = transform.GetComponentFromParentRecursive<Rigidbody2D>(true);
        m_controller = transform.GetComponentFromParentRecursive<CharacterController2D>(true);
        m_uid = transform.GetComponentFromParentRecursive<UID>(true);
        m_animator = GetComponent<Animator>();

        InputController inputController = transform.GetComponentFromParentRecursive<InputController>();
        if(inputController != null)
        {
            InputController.ButtonDownHandler jumpPressed = OnJumpPressed;
            InputController.ButtonUpHandler jumpReleased = OnJumpReleased;
            inputController.RegisterHandler("Jump", this, jumpPressed);
            inputController.RegisterHandler("Jump", this, jumpReleased);

            InputController.ButtonDownHandler dashPressed = OnDashPressed;
            InputController.ButtonUpHandler dashReleased = OnDashReleased;
            inputController.RegisterHandler("Dash", this, dashPressed);
            inputController.RegisterHandler("Dash", this, dashReleased);
        }
    }

    void OnEnable()
    {
        MessageSystem<OnExitDash>.Register(this, OnExitDash);
    }

    void OnDisable()
    {
        MessageSystem<OnExitDash>.Remove(this, OnExitDash);
    }

	void Update()
    {
        float x = Mathf.Abs(m_body.velocity.x);
        m_animator.SetBool("moving", x > 0.01f);
        m_animator.SetFloat("yDir", m_body.velocity.y);
        m_animator.SetBool("grounded", m_controller.grounded);
	}

    void OnJumpPressed()
    {
        if(m_controller.grounded)
        {
            m_animator.SetTrigger("jump");
        }
    }

    void OnJumpReleased()
    {
        m_animator.ResetTrigger("jump");
    }

    void OnDashPressed()
    {
        if(m_controller.grounded)
        {
            m_animator.SetBool("dash", true);
        }
    }

    void OnDashReleased()
    {
        m_animator.SetBool("dash", false);
    }

    void OnExitDash(OnExitDash evt)
    {
        if(m_uid.value == evt.uid)
        {
            m_animator.SetBool("dash", false);
        }
    }
}
