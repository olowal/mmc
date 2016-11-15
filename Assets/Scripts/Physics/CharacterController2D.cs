using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    // Remove this later
    public GameObject m_bullet = null;

    [SerializeField]
    private LayerMask m_layerMask;
    [SerializeField]
    private float m_groundGravityScale = 20.0f;
    [SerializeField]
    private float m_inAirGravityScale = 5.0f;
    [SerializeField]
    private float m_slopeFriction = 0.5f;
    [SerializeField]
    private float m_jump = 200.0f;
    [SerializeField]
    private float m_speed = 10.0f;
    [SerializeField]
    private float m_speedMultiplier = 1.0f;

    private float m_xVel = 0.0f;

    private Rigidbody2D m_body = null;
    private UID m_uid = null;

    private bool m_grounded = false;
    private bool m_wasGrounded = false;
    private bool m_isJmping = true;

    private float m_xDir = 1.0f;

    public bool grounded { get { return m_grounded; } }
    public bool isJumping { get { return m_isJmping; } }
    public float movement { get { return m_xVel; } set { m_xVel = value; } }
    public float speedMultiplier { get { return m_speedMultiplier; } set { m_speedMultiplier = value; } }
    public float facingDirection { get { return m_xDir; } }
    public bool isMoving { get { return Mathf.Abs(m_body.velocity.x) > 0.0f; } }

    void Awake()
    {
        m_body = GetComponent<Rigidbody2D>();
        m_uid = GetComponent<UID>();
    }
	
	void FixedUpdate()
    {
        float dt = Time.deltaTime;
        m_body.velocity = new Vector2(m_xVel * (m_speed * m_speedMultiplier) * dt, m_body.velocity.y);

        if(m_xVel != 0.0f)
        {
            m_xDir = m_xVel;
        }

        if (m_body.velocity.y < 0.0f)
        {
            m_isJmping = false;
        }

        NormalizeSlope();

        if(m_grounded && !m_wasGrounded)
        {
            m_body.gravityScale = m_groundGravityScale;
            if(m_uid != null)
            {
                MessageSystem<OnExitAirEvent>.Send(new OnExitAirEvent(m_uid.value));
            }
        }
        else if(m_wasGrounded && !m_grounded)
        {
            m_body.gravityScale = m_inAirGravityScale;
            if(m_uid != null)
            {
                MessageSystem<OnEnterAirEvent>.Send(new OnEnterAirEvent(m_uid.value));
            }
        }
        m_wasGrounded = m_grounded;
    }

    void NormalizeSlope()
    {
        if(!m_isJmping)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1.5f, m_layerMask.value);
            //Debug.DrawRay(transform.position, -Vector3.up, Color.red);
            if(hit.collider != null && Mathf.Abs(hit.normal.x) > 0.1f)
            {
                m_body.velocity = new Vector2(m_body.velocity.x - (hit.normal.x * m_slopeFriction), m_body.velocity.y);
                m_grounded = true;
            }
            else if(hit.collider != null)
            {
                m_grounded = true;
            }
            else
            {
                m_grounded = false;
            }
        }
    }

    public void Jump()
    {
        m_body.velocity = new Vector2(m_body.velocity.x, m_jump);
        m_grounded = false;
        m_isJmping = true;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(m_isJmping)
        {
            ContactPoint2D[] contacts = coll.contacts;
            for (int i = 0; i < contacts.Length; ++i)
            {
                if (contacts[i].normal.y < -0.1f)
                {
                    m_isJmping = false;
                    break;
                }
            }
        }
    }
}
