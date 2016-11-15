using UnityEngine;
using System.Collections;

public struct OnExitAirEvent
{
    public OnExitAirEvent(int uid)
    {
        m_uid = uid;
    }
    public int uid { get { return m_uid; } }
    private int m_uid;
}
