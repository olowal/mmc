using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class KinematicPhysics : MonoBehaviour
{
    [SerializeField]
    private bool m_limitVelovity = false;
    [SerializeField]
    private Vector2 m_maxVelocity = Vector2.zero;
    private Vector2 m_vel = Vector2.zero;
    private Rigidbody2D m_body = null;

    public bool LimitVelocity
    {
        get { return m_limitVelovity; }
        set { m_limitVelovity = value; }
    }
	
	void Awake()
    {
        if(m_limitVelovity)
        {
            m_vel.x = Mathf.Clamp(m_vel.x, -m_maxVelocity.x, m_maxVelocity.x);
            m_vel.y = Mathf.Clamp(m_vel.y, -m_maxVelocity.y, m_maxVelocity.y);
        }
        
        m_body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update()
    {
        m_body.velocity = m_vel;
	}

    public void Push(Vector2 dir)
    {
        m_vel += dir;
    }
}
