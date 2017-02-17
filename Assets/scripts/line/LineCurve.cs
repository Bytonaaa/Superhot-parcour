using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineCurve
{
    [SerializeField] private Vector3[] points;
    [SerializeField] [Range(0.1f, 15f)] private float[] time;
    [SerializeField] [Range(0f, 15f)] private float[] waitTime;

    public Vector3 getPosition(int from, int to, float t)
    {
        return Vector3.Lerp(points[from], points[to], t);
    }

    public float getTime(int from, int to)
    {
        return (from < to ?  time[from] : time[to]);
    }

    public float getWaitTime(int pos)
    {
        return waitTime[pos];
    }

    public bool isEnd(int to)
    {
        return to < 0 || to >= points.Length;
    }
}
