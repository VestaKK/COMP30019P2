using UnityEngine;
using System.Collections.Generic;
public class RoomNode : Node
{
    public bool IsSpawn { get; set; }
    public Vector2 SpawnPoint { get; set; }
    public bool IsExit { get; set; }
    public Vector2 ExitPoint { get; set; }
    public List<(CorridorNode,RoomNode)> ConnectedNodes { get; set; }
    public List<Door> Doors { get; set; }
    public List<Prop> Props { get; set; }
    public RoomType Type { get; set; }

    public RoomNode(
        Vector2Int bottomLeftAreaCorner, 
        Vector2Int topRightAreaCorner, 
        Node parentNode, 
        int index) : base(parentNode)
    {
        this.BottomLeftAreaCorner = bottomLeftAreaCorner;
        this.TopRightAreaCorner = topRightAreaCorner;
        this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y);
        this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, topRightAreaCorner.y);

        this.TreeLayerIndex = index;

        ConnectedNodes = new List<(CorridorNode, RoomNode)>();
        Props = new List<Prop>();
        Doors = new List<Door>();
    }

    public int Width { get => (int) (TopRightAreaCorner.x - BottomLeftAreaCorner.x); }
    public int Length { get => (int) (TopRightAreaCorner.y - BottomLeftAreaCorner.y); }
}

public enum RoomType 
{
    Crossway,
    Hallway,
    BeegRoom,
    StorageRoom,
    LabRoom,
    ComputerRoom,
    Library,
    ChallengeRoom,
    SpawnRoom,
    ExitRoom
}