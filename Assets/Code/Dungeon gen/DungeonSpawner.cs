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
        var listOfRooms = generator.CalculateRoomsAndCorridors(
            maxIterations, 
            roomWidthMin, 
            roomLengthMin, 
            bottomCornerModifier, 
            topCornerModifier, 
            offset, 
            corridorWidth, 
            distanceFromWall
        );

        // Instantiate object which will be parent of all walls,
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();

        // Create mesh for rooms
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }

        // Instantiate walls
        CreateWalls(wallParent);

        // Randomise spawn and exit room
        CreateSpawnAndExitPoint(listOfRooms);
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

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i=0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
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

    // Randomise one of the four corner rooms as a exit point
    // Find centermost room and spawn player in there
    private void CreateSpawnAndExitPoint(List<Node> listOfRooms)
    {
        Vector3 playerSpawnPoint;
        Vector3 exitPoint;
        Node exitRoom;

        // Find room center closest to center of dungeon, set as spawn
        Vector2 spawnRoomMiddle = 
            listOfRooms.Select(
                child => StructureHelper.CalculateMiddlePoint(
                    child.BottomLeftAreaCorner, 
                    child.TopRightAreaCorner)
            ).OrderBy(
                child => Vector2.Distance(child, new Vector2(dungeonWidth/2, dungeonLength/2))
            ).ToList()[0];
        playerSpawnPoint = new Vector3(spawnRoomMiddle.x, 0, spawnRoomMiddle.y);

        // Randomly select exit room between four corners
        int corner = Random.Range(0,4);
        if (corner == 0) // Bottom left corner exit
        {   
            exitRoom = 
                listOfRooms.OrderBy(
                    child => child.BottomLeftAreaCorner.x + child.BottomLeftAreaCorner.y
                ).ToList()[0];
        }
        else if (corner == 1) // Bottom Right corner exit
        {
            exitRoom = 
                listOfRooms.OrderBy(
                    child => - child.BottomLeftAreaCorner.x + child.BottomLeftAreaCorner.y
                ).ToList()[0];
        }
        else if (corner == 2) // Top Left corner exit
        {
            exitRoom = 
                listOfRooms.OrderBy(
                    child => child.BottomLeftAreaCorner.x - child.BottomLeftAreaCorner.y
                ).ToList()[0];
        }
        else // Top Right corner exit
        {
            exitRoom = 
                listOfRooms.OrderByDescending(
                    child => child.BottomLeftAreaCorner.x + child.BottomLeftAreaCorner.y
                ).ToList()[0];
        }

        Vector2 exitRoomMiddle = 
            StructureHelper.CalculateMiddlePoint(
                exitRoom.BottomLeftAreaCorner, 
                exitRoom.TopRightAreaCorner);
        exitPoint = new Vector3(exitRoomMiddle.x, 0, exitRoomMiddle.y);

        // Debug purposes
        Debug.Log(playerSpawnPoint);
        Debug.Log(exitPoint);
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
