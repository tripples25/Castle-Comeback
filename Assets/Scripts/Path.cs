using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public List<Vector3> Points;
    public float Length;

    public Path()
    {
        Points = new();
        Length = 0;
    }
}