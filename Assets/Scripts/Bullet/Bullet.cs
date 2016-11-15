using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float m_dir = 1.0f;
    public float m_speed = 20.0f;

	void Start ()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(m_dir * 20.0f, 0.0f);
	}
}
