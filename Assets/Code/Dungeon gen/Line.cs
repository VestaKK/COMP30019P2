using UnityEngine;

// Class representing a line
public class Line
{
    private Orientation orientation;
    private Vector2Int coordinates;

    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}

// Enum for orientation
public enum Orientation
{
    Horizontal = 0,
    Vertical = 1
}