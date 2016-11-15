using UnityEngine;

public static class MathEx
{
    public static float Sign01(float v, float min = 0.1f)
    {
        float max = Mathf.Abs(min);
        min = max * -1.0f; 

        if(v > min && v < max)
        {
            return 0.0f;
        }

        if(v < min) { return -1.0f; }
        return 1.0f;
    }

    private static readonly float s_angle = 180.0f;
    public static float RotateAngle(float a, float v)
    {
        float angle = a + v;
        if(angle > s_angle)
        {
            angle = (-s_angle) + (angle - s_angle);
        }
        else if(angle < (-s_angle))
        {
            angle = s_angle - (angle - s_angle);
        }

        return angle;
    }
}
