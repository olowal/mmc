using UnityEngine;

public class EdgeColliderRenderer2D : MonoBehaviour {

    // Use this for initialization
    private LineRenderer m_lineRenderer = null;
    private EdgeCollider2D m_edgeCollider = null;
	void Start ()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_edgeCollider = GetComponent<EdgeCollider2D>();
        if(!m_edgeCollider)
        {
            m_edgeCollider = transform.GetComponentFromParentRecursive<EdgeCollider2D>();
        }

        if(m_edgeCollider && m_lineRenderer)
        {
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            m_lineRenderer.receiveShadows = false;
            int count = m_edgeCollider.pointCount;
            m_lineRenderer.SetVertexCount(count);
            Vector2[] points = m_edgeCollider.points;
            float z = 1.0f;
            for (int i = 0; i < count; ++i)
            {
                Vector2 p = points[i];
                m_lineRenderer.SetPosition(i, new Vector3(p.x, p.y, z));
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
