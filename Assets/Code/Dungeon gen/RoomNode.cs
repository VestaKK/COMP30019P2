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
            Vector3 spawnPoint = GetSafeSpawn(base_spawn, spawner.PrefabController);
            Enemy enemy = spawner.SpawnEntity(_dungeonTransform, spawnPoint, Quaternion.identity);
        }
    }

    private Vector3 GetSafeSpawn(Vector3 base_spawn, CharacterController prefab) {
        Vector3 randSpawn = GetRandSpawn(base_spawn, prefab);
        Collider[] collided = Physics.OverlapSphere(randSpawn, prefab.radius);
        while(collided.Length != 0) {
            Debug.Log("Bad spawn. Recalc");
            randSpawn = GetRandSpawn(base_spawn, prefab);
            collided = Physics.OverlapSphere(randSpawn, prefab.radius);
        }

        Debug.Log("Safe spawn found");
        return randSpawn;
    }

    private Vector3 GetRandSpawn(Vector3 base_spawn, CharacterController prefab) {
        float randX = Random.Range(-SpawnBounds.width / 2, SpawnBounds.width / 2);
        float randZ = Random.Range(-SpawnBounds.height / 2, SpawnBounds.height / 2);
        return new Vector3(base_spawn.x + randX, prefab.center.y + prefab.radius + 0.25f, base_spawn.z + randZ); // raise it off the floor
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