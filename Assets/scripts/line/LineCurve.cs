using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineCurve
{
    [SerializeField] private Vector3[] points;
    [SerializeField] [Range(0.01f, 100f)] private float[] speed;


    public Vector3 getPosition(int from, int to, float t)
    {
        return Vector3.Lerp(points[from], points[to], t);
    }

    public float getSpeed(int from, int to)
    {
        return from < to ? speed[from] : speed[to];
    }

    public bool isEnd(int to)
    {
        return to < 0 || to >= points.Length;
    }
}
