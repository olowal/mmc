using UnityEngine;
using System.Collections;

public class SpriteFlipper : MonoBehaviour
{
    [SerializeField]
    private bool m_flip = true;
    private SpriteRenderer m_sprite = null;
    private CharacterController2D m_controller = null;
    private float m_xDir = 0.0f;
    private float m_lastXDir = 0.0f;
    
	void Awake ()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_controller = transform.GetComponentFromParentRecursive<CharacterController2D>();
        InputController inputController = transform.GetComponentFromParentRecursive<InputController>(true);
        if(inputController != null)
        {
            InputController.AxisHandler axisHandler = OnAxisMoved;
            inputController.RegisterHandler("Horizontal", this, axisHandler);
        }
	}

    void Start()
    {
        //if(m_listenForEvents)
        {
            //SetupEventListeners();
        }
    }

    void OnDestroy()
    {
        //if(m_listenForEvents)
        {
           // RemoveEventListeners();
        }
    }
	
	void Update ()
    {
        if(m_controller != null)
        {
            if(!m_controller.grounded)
            {
                return;
            }
        }
        float xVel = m_xDir;
        bool notMoving = xVel == 0.0f;
        
        if(xVel > m_lastXDir && !notMoving)
        {
            m_sprite.flipX = m_flip;
        }
        else if(xVel < m_lastXDir && !notMoving)
        {
            m_sprite.flipX = !m_flip;
        }
        m_lastXDir = xVel;
	}

    void SetupEventListeners()
    {
        MessageSystem<OnEnterAirEvent>.Register(this, OnEnterAir);
        MessageSystem<OnExitAirEvent>.Register(this, OnExitAir);
    }

    void RemoveEventListeners()
    {
        MessageSystem<OnEnterAirEvent>.Remove(this, OnEnterAir);
        MessageSystem<OnExitAirEvent>.Remove(this, OnExitAir);
    }

    void OnEnterAir(OnEnterAirEvent evt)
    {
        enabled = true;
    }

    void OnExitAir(OnExitAirEvent evt)
    {
        enabled = false;
    }

    void OnAxisMoved(float value)
    {
        m_xDir = value;
    }
}
