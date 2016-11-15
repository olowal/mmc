using UnityEngine;
using System.Collections;

public struct OnEnterAirEvent
{
    public OnEnterAirEvent(int uid)
    {
        m_uid = uid;
    }
    public int uid { get { return m_uid; } }
    private int m_uid;
}
