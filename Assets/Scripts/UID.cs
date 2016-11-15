using UnityEngine;
using System.Collections;

public class UID : MonoBehaviour
{
    static int s_uid = 0;

    private int m_uid;
    public int value { get { return m_uid; } }

    void Awake()
    {
        m_uid = s_uid;
        s_uid++;
    }
}
