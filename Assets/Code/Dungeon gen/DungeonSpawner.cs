using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random; 

public class DungeonSpawner: MonoBehaviour
{
     // Width & Length of entire dungeon floor
    [SerializeField] private int dungeonWidth, dungeonLength;
    // Min room width & length
    [SerializeField] private int roomWidthMin, roomLengthMin; 
    // Spawn room size
    [SerializeField] private int spawnRoomWidth, spawnRoomLength;
    // Exit room size
    [SerializeField] private int exitRoomWidth, exitRoomLength;
    // Multiplier for how far the separation between the space allocated and the room is at bottom left corner
    [SerializeField] [Range(0.0f, 0.3f)] private float bottomCornerModifier; 
    // Multiplier for how far the separation between the space allocated and the room is at top right corner
    [SerializeField] [Range(0.7f, 1.0f)] private float topCornerModifier;
    // Offset between rooms
    [SerializeField] [Range(0, 3)] private int offset;
    // Max no. of iterations to partition
    [SerializeField] private int maxIterations;
    // Width of corridor
    [SerializeField] private int corridorWidth;
    // Distance from room wall to corridor
    [SerializeField] private int distanceFromWall;

    // Floor material
    [SerializeField] private Material floorMaterial;

    // Game object for vertical & horizontal walls
    [SerializeField] GameObject wallVertical, wallHorizontal;

    // Possible positions for a wall/door
    // Currently no implementation for doors
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;

    public List<Node> listOfNodes;

    void Start()
    {
        SpawnDungeon();
    }

    // Spawn a dungeon
    public void SpawnDungeon() 
    {
        // Destroy previous instance of dungeon
        DestroyAllChildren();

        // Generate dungeon rooms
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        listOfNodes = generator.CalculateRoomsAndCorridors(
            maxIterations, 
            roomWidthMin, 
            roomLengthMin, 
            spawnRoomWidth,
            spawnRoomLength,
            exitRoomWidth,
            exitRoomLength,
            bottomCornerModifier, 
            topCornerModifier, 
            offset, 
            corridorWidth, 
            distanceFromWall
        );

        // DEBUG purposes
        List<RoomNode> listRooms = 
            listOfNodes.FindAll(node => node is RoomNode)
            .Select(node => (RoomNode) node).ToList();
        RoomNode spawn = listRooms.Find((room) => room.IsSpawn);
        Debug.Log(spawn.SpawnPoint);
        RoomNode exit = listRooms.Find((room) => room.IsExit);
        Debug.Log(exit.ExitPoint);

        // Instantiate object which will be parent of all walls,
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();

        // Create mesh for rooms
        for (int i = 0; i < listOfNodes.Count; i++)
        {
            CreateMesh(listOfNodes[i].BottomLeftAreaCorner, listOfNodes[i].TopRightAreaCorner);
        }

        // Instantiate walls
        CreateWalls(wallParent);
    }

    // Function to create meshes for a room
    private void CreateMesh(Vector2Int bottomLeft, Vector2Int topRight)
    {
        Vector3 bottomLeftVertex = new Vector3(bottomLeft.x, 0, bottomLeft.y);
        Vector3 bottomRightVertex = new Vector3(topRight.x, 0, bottomLeft.y);
        Vector3 topLeftVertex = new Vector3(bottomLeft.x, 0, topRight.y);
        Vector3 topRightVertex = new Vector3(topRight.x, 0, topRight.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftVertex,
            topRightVertex,
            bottomLeftVertex,
            bottomRightVertex
        };

        Vector3[] normals = new Vector3[]
        {
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0)
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i=0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            2, 1, 3,
            0, 1, 2
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.RecalculateTangents();
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject(
            "Mesh" + bottomLeft, 
            typeof(MeshFilter), 
            typeof(MeshRenderer));
        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = floorMaterial;
        dungeonFloor.transform.parent = this.transform;

        // Add positions where walls belong to list
        for (int row = (int) bottomLeftVertex.x; row < (int) bottomRightVertex.x; row++)
        {
            Vector3 wallPosition = new Vector3(row, 0, bottomLeftVertex.z);
            AddWallPositionToList(
                wallPosition, 
                possibleWallHorizontalPosition, 
                possibleDoorHorizontalPosition);
        }

        for (int row = (int) topLeftVertex.x; row < topRightVertex.x; row++)
        {
            Vector3 wallPosition = new Vector3(row, 0, topRightVertex.z);
            AddWallPositionToList(
                wallPosition, 
                possibleWallHorizontalPosition, 
                possibleDoorHorizontalPosition);
        }

        for (int col = (int) bottomLeftVertex.z; col < topLeftVertex.z; col++)
        {
            Vector3 wallPosition = new Vector3(bottomLeftVertex.x, 0, col);
            AddWallPositionToList(
                wallPosition, 
                possibleWallVerticalPosition, 
                possibleDoorVerticalPosition);
        }
        for (int col = (int) bottomRightVertex.z; col < topRightVertex.z; col++)
        {
            Vector3 wallPosition = new Vector3(bottomRightVertex.x, 0, col);
            AddWallPositionToList(
                wallPosition, 
                possibleWallVerticalPosition, 
                possibleDoorVerticalPosition);
        }
    }

    // Given a wall of a room, add/remove wall and door accordingly
    private void AddWallPositionToList(
        Vector3 wallPosition, 
        List<Vector3Int> wallList, 
        List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point)) // If wall overlaps with one from a room already iterated through
        {
            // Delete said wall and replace with door
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            // Add as wall
            wallList.Add(point);
        }
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach(Transform item  in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
    
}
