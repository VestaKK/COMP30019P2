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
        Vector3 base_spawn = new Vector3(MiddlePoint.x, 0, MiddlePoint.y);
        while(spawns++ < targetSpawns) {
            Vector3 spawnPoint = GetSafeSpawn(base_spawn, spawner.PrefabController.center, spawner.PrefabController.radius);
            Debug.Log("Spawning object with center: " + spawner.PrefabController.center);
            Debug.Log("Radius: " + spawner.PrefabController.radius);
            Enemy enemy = spawner.SpawnEntity(_dungeonTransform, spawnPoint, Quaternion.identity);
        }
    }

    private Vector3 GetSafeSpawn(Vector3 base_spawn, Vector3 objCenter, float radius) {
        Vector3 randSpawn = GetRandSpawn(base_spawn, objCenter);
        Collider[] collided = Physics.OverlapSphere(randSpawn, radius);
        while(collided.Length != 0) {
            randSpawn = GetRandSpawn(base_spawn, objCenter);
            collided = Physics.OverlapSphere(randSpawn, radius);
        }
        return randSpawn;
    }

    private Vector3 GetRandSpawn(Vector3 base_spawn, Vector3 objCenter) {
        float randX = Random.Range(-SpawnBounds.width / 2, SpawnBounds.width / 2);
        float randZ = Random.Range(-SpawnBounds.height / 2, SpawnBounds.height / 2);
        return new Vector3(base_spawn.x + randX, objCenter.y + 5, base_spawn.z + randZ);
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