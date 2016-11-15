
public struct Pair<T, U>
{
    public Pair(T First, U Second)
    {
        m_first = First;
        m_second = Second;
    }

    public T First { get { return m_first; } set { m_first = value; } }
    public U Second { get { return m_second; } set { m_second = value; } }

    public T m_first;
    public U m_second;
}
