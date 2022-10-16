using UnityEngine;
using System.Collections.Generic;
public class RoomNode : Node
{
    public bool IsSpawn { get; set; }
    public Vector2 SpawnPoint { get; set; }
    public bool IsExit { get; set; }
    public Vector2 ExitPoint { get; set; }

    public Vector2 MiddlePoint { get; set; }

    public List<(CorridorNode,RoomNode)> ConnectedNodes { get; set; }
    public List<Door> Doors { get; set; }
    public List<Prop> Props { get; set; }
    public RoomType Type { get; set; }

    // Enemy stuff
    public int EnemyCount { get; set; }
    public float _roomDifficulty = 0f;

    private Transform _dungeonTransform;

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

    public void SpawnEnemies<T>(Spawner<T> spawner) where T : Enemy {
        int spawns = 0;
        int targetSpawns = spawner.GetSpawnCount();
        while(spawns++ < targetSpawns) {
            Enemy enemy = spawner.SpawnEntity(_dungeonTransform, new Vector3(MiddlePoint.x, 10, MiddlePoint.y), Quaternion.identity);
            Debug.Log("Spawning: " + enemy);
            Debug.Log("Spawning: " + enemy.Position);
        }
    }

    public int Width { get => (int) (TopRightAreaCorner.x - BottomLeftAreaCorner.x); }
    public int Length { get => (int) (TopRightAreaCorner.y - BottomLeftAreaCorner.y); }

    public Transform DungeonTransform { get => _dungeonTransform; set => _dungeonTransform = value; }
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