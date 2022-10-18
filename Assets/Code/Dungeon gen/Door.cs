using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door
{
    public Vector2Int coordinates { get; set; }
    public Orientation orientation { get; set; }

    public Door(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }
}
