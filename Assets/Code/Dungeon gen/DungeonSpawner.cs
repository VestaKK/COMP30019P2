using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random; 

public class DungeonSpawner: MonoBehaviour
{
    [SerializeField] public NavMeshSurface NavMesh;

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

    // Floor & Ceiling material
    [SerializeField] private Material floorMaterial, ceilingMaterial;
    // extra circumference to dungeon to add ceiling to
    [SerializeField] private int extraCeilingWidth = 30;

    // Light colors for room ceiling lights
    [SerializeField] private Color[] ceilingLightColors;

    // Game object for walls
    [SerializeField] public GameObject wallObject;

    [SerializeField] private float bigRoomMultiplier;
    [SerializeField] public GameObject spawnFloorTile, spawnFloorTileEdge, spawnFloorTileCorner;
    [SerializeField] public GameObject exitObject;
    [SerializeField] public GameObject pillarObject;
    [SerializeField] public GameObject[] shelvesObjectsList;
    [SerializeField] public GameObject crateObject;
    [SerializeField] public GameObject computerObject;
    [SerializeField] public GameObject[] labItemObjectList;
    [SerializeField] public GameObject torchObject;
    [SerializeField] public GameObject statueObject;
    [SerializeField] public GameObject[] cornerPropsList;

    [SerializeField] private EnemySpawner enemySpawner;

    // Spawn a dungeon
    public DungeonController SpawnDungeon() 
    {
        List<Node> listOfNodes;

        // Create Dungeon parent object
        GameObject dungeonObject = new GameObject("Dungeon");
        DungeonController dungeonController = dungeonObject.AddComponent<DungeonController>();

        // Generate dungeon rooms
        DungeonGenerator generator = new DungeonGenerator(dungeonController, dungeonWidth, dungeonLength);
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

        // Populate room with props
        List<RoomNode> listRooms = 
            listOfNodes.FindAll(node => node is RoomNode)
            .Select(node => (RoomNode) node).ToList();
        foreach (RoomNode room in listRooms)
        {
            PopulateWithProps(room);
        }

        // Attach rooms to dungeon controller
        dungeonController.rooms = listRooms;
        dungeonController.spawnRoom = listRooms.Find((room) => room.Type == RoomType.SpawnRoom);
        dungeonController.exitRoom = listRooms.Find((room) => room.Type == RoomType.ExitRoom);

        List<CorridorNode> listCorridors = 
            listOfNodes.FindAll(node => node is CorridorNode)
            .Select(node => (CorridorNode) node).ToList();
        dungeonController.corridors = listCorridors;

        // spawn walls, floors, props & lights for rooms & corridors
        for (int i = 0; i < listRooms.Count; i++)
        {
            InstantiateRoom(listRooms[i], i, dungeonObject);
        }
        for (int i = 0; i < listCorridors.Count; i++)
        {
            InstantiateCorridor(listCorridors[i], i, dungeonObject);
        }

        // Spawn ceilings
        GameObject ceilingParent = new GameObject("CeilingParent");
        ceilingParent.transform.parent = dungeonObject.transform;
        bool[,] ceilingPositions = new bool[dungeonWidth + 2 * extraCeilingWidth, dungeonLength + 2 * extraCeilingWidth];
        for (int w = 0; w < ceilingPositions.GetLength(0); w++)
            for (int l = 0; l < ceilingPositions.GetLength(1); l++)
                ceilingPositions[w,l] = true;
        
        listOfNodes.ForEach((node) => {
            for (int w = node.BottomLeftAreaCorner.x; w < node.TopRightAreaCorner.x; w++)
                for (int l = node.BottomLeftAreaCorner.y; l < node.TopRightAreaCorner.y; l++)
                    ceilingPositions[w + extraCeilingWidth, l + extraCeilingWidth] = false;
        });
        for (int w = 0; w < ceilingPositions.GetLength(0); w++)
            for (int l = 0; l < ceilingPositions.GetLength(1); l++)
                if (ceilingPositions[w,l])
                    CreateCeiling(w - extraCeilingWidth, l - extraCeilingWidth, ceilingParent);

        NavMesh.BuildNavMesh();

        // Enemy spawns
        // good to have a list of enemies. Dunno what to do with them yet though
        dungeonController.rooms.ForEach((room) => {
            if(room.Type == RoomType.SpawnRoom)
                return;
            room.SpawnEnemies(enemySpawner);
        });

        

        return dungeonController;
    }

    private void InstantiateRoom(RoomNode room, int roomIndex, GameObject dungeonObject)
    {
        GameObject roomObject = new GameObject("Room " + roomIndex);
        CreateFloor(room.BottomLeftAreaCorner, room.TopRightAreaCorner, roomObject);

        GameObject wallParent = new GameObject("WallParent");
        CreateWalls(room, wallParent);
        wallParent.transform.parent = roomObject.transform;
        
        SpawnCeilingLight(room, roomObject);

        foreach (Prop prop in room.Props)
        {
            GameObject newProp = Instantiate(
                prop.propObject,
                prop.coordinates,
                prop.rotation,
                roomObject.transform
            );

            newProp.AddComponent<BoxCollider>();
        }

        roomObject.transform.parent = dungeonObject.transform;
    }

    public void InstantiateCorridor(CorridorNode corridor, int corridorIndex, GameObject dungeonObject)
    {
        GameObject corridorObject = new GameObject("Corridor " + corridorIndex);
        CreateFloor(corridor.BottomLeftAreaCorner, corridor.TopRightAreaCorner, corridorObject);

        GameObject wallParent = new GameObject("WallParent");
        CreateWalls(corridor, wallParent);
        wallParent.transform.parent = corridorObject.transform;

        corridorObject.transform.parent = dungeonObject.transform;
    }

    // Function to create floor meshes for a room/corridor
    private void CreateFloor(Vector2Int bottomLeft, Vector2Int topRight, GameObject parent)
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

        CreatePlane(vertices, parent, this.floorMaterial, "Floor");
    }

    // Create plane for floor/ceiling
    private void CreatePlane(Vector3[] vertices, GameObject parent, Material material, string objectName)
    {
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

        GameObject plane = new GameObject(
            objectName, 
            typeof(MeshFilter), 
            typeof(MeshRenderer));
        plane.transform.position = Vector3.zero;
        plane.transform.localScale = Vector3.one;
        plane.GetComponent<MeshFilter>().mesh = mesh;
        plane.GetComponent<MeshRenderer>().material = material;
        plane.transform.parent = parent.transform;

        if (objectName != "Ceiling")
            plane.AddComponent<BoxCollider>();
    }
    
    // Create walls for a room/corridor
    private void CreateWalls(Node node, GameObject parent)
    {
        foreach (Wall wall in node.Walls)
        {
            Vector3 wallPosition = new Vector3(wall.coordinates.x, 0, wall.coordinates.y);
            GameObject newWall = Instantiate(
                wallObject, 
                wallPosition + new Vector3(0f, -0.1f, 0f), 
                wall.orientation == Orientation.Horizontal ? Quaternion.Euler(0f, -90f, 0f) : Quaternion.Euler(0f, -180f, 0f),
                parent.transform);
            newWall.AddComponent<BoxCollider>();
        }
    }

    // Create ceiling at a location
    private void CreateCeiling(int x, int y, GameObject parent)
    {
        float wallHeight = PropsHelper.GetObjectBounds(wallObject).y - 0.09f;
        Vector3 bottomLeftVertex = new Vector3(x, wallHeight, y);
        Vector3 bottomRightVertex = new Vector3(x+1, wallHeight, y);
        Vector3 topLeftVertex = new Vector3(x, wallHeight, y+1);
        Vector3 topRightVertex = new Vector3(x+1, wallHeight, y+1);
        
        Vector3[] vertices = new Vector3[]
        {
            topLeftVertex,
            topRightVertex,
            bottomLeftVertex,
            bottomRightVertex
        };

        CreatePlane(vertices, parent, this.ceilingMaterial, "Ceiling");
    }

    private void SpawnCeilingLight(RoomNode room, GameObject parent)
    {
        // Make a game object
        GameObject lightGameObject1 = new GameObject("Room Light 1");

        Color color = ceilingLightColors[Random.Range(0, ceilingLightColors.Length)];

        // Add the light component
        Light lightComp1 = lightGameObject1.AddComponent<Light>();

        lightComp1.color = color;
        lightComp1.intensity = 4;

        Vector2 center = room.Center;

        if (
            room.Type == RoomType.SpawnRoom || 
            room.Type == RoomType.ExitRoom || 
            (room.Width < roomWidthMin && room.Length < roomLengthMin))
        {
            lightGameObject1.transform.position = new Vector3(center.x, 4, center.y);
            lightGameObject1.transform.parent = parent.transform;
        }
        else 
        {
            // Make a game object
            GameObject lightGameObject2 = new GameObject("Room Light 2");

            // Add the light component
            Light lightComp2 = lightGameObject2.AddComponent<Light>();

            lightComp2.color = color;
            lightComp2.intensity = 4;

            // Space light evenly according to room length/width
            if (room.Width > room.Length)
            {
                lightGameObject1.transform.position = new Vector3(center.x - room.Width / 5.0f, 4, center.y);
                lightGameObject2.transform.position = new Vector3(center.x + room.Width / 5.0f, 4, center.y);
            }
            else
            {
                lightGameObject1.transform.position = new Vector3(center.x, 4, center.y - room.Length / 5.0f);
                lightGameObject2.transform.position = new Vector3(center.x, 4, center.y + room.Length / 5.0f);
            }

            lightGameObject1.transform.parent = parent.transform;
            lightGameObject2.transform.parent = parent.transform;
        }
    }

    // Assign room types and populate rooms with props accordingly
    private void PopulateWithProps(RoomNode room)
    {
        Vector2 center = room.Center;

        // separate rooms into different types to spawn respective props
        if (room.Type == RoomType.SpawnRoom)
        {
            room.Type = RoomType.SpawnRoom;
            PropsHelper.PopulateSpawnRoom(room, spawnFloorTile, spawnFloorTileEdge, spawnFloorTileCorner);
        }
        else if (room.Type == RoomType.ExitRoom)
        {
            room.Type = RoomType.ExitRoom;
            PropsHelper.PopulateExitRoom(room, exitObject, torchObject);
        }
        else if (
            room.Width >= this.roomWidthMin * this.bigRoomMultiplier &&
            room.Length >= this.roomLengthMin * this.bigRoomMultiplier) // Big room
        {
            room.Type = RoomType.BeegRoom;
            PropsHelper.PopulateBeegRoom(room, pillarObject, statueObject, cornerPropsList);
        }
        else if (
            room.Width >= 1.7 * room.Length &&
            room.ConnectedNodes.Exists(
                pair => 
                    (int) StructureHelper.GiveRelativePosition(center, pair.Item1.Center) >= 2)) // left/right position
        {
            room.Type = RoomType.Hallway;
            PropsHelper.PopulateHallway(room, Orientation.Horizontal, pillarObject, cornerPropsList);
        }
        else if (room.Length >= 1.7 * room.Width &&
            room.ConnectedNodes.Exists(
                pair => 
                    (int) StructureHelper.GiveRelativePosition(center, pair.Item1.Center) <= 1)) // up/down position
        {
            room.Type = RoomType.Hallway;
            PropsHelper.PopulateHallway(room, Orientation.Vertical, pillarObject, cornerPropsList);
        }
        else if (room.Width >= this.roomWidthMin && room.Length >= this.roomLengthMin)
        {
            room.Type = RoomType.ComputerRoom;
            PropsHelper.PopulateComputerRoom(room, computerObject, spawnFloorTile, pillarObject, cornerPropsList);
        }
        else if (
            room.ConnectedNodes.Count == 1 &&
            room.Width <= this.roomWidthMin * this.bigRoomMultiplier &&
            room.Length <= this.roomLengthMin * this.bigRoomMultiplier) 
        { // Dead End
            room.Type = RoomType.StorageRoom;
            PropsHelper.PopulateStorageRoom(room, shelvesObjectsList);
        }
        else
        {
            RoomType[] roomTypes = 
            {
                RoomType.LabRoom,
                RoomType.Library
            };
            room.Type = roomTypes[Random.Range(0, roomTypes.Length)];
            switch (room.Type)
            {
                case (RoomType.LabRoom):
                    PropsHelper.PopulateLabRoom(room, labItemObjectList, computerObject, spawnFloorTile, cornerPropsList);
                    break;
                case (RoomType.Library):
                    PropsHelper.PopulateLibrary(room, shelvesObjectsList, computerObject, spawnFloorTile);
                    break;
                default:
                    break;
            }
        }
    }

}
