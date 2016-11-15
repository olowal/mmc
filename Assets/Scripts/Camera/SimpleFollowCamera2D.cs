using UnityEngine;
using System.Collections;

public class SimpleFollowCamera2D : MonoBehaviour
{
    [SerializeField]
    private Transform m_target = null;

    public Transform target { get { return m_target; } set { m_target = value; } }

	void LateUpdate ()
    {
	    if(m_target == null)
        {
            return;
        }

        Vector3 targetPosition = m_target.position;
        targetPosition.z = -1.0f;
        transform.position = targetPosition;
	}
}
