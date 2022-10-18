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

    // Floor material
    [SerializeField] private Material floorMaterial;

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
    [SerializeField] public GameObject[] propsList;

    [SerializeField] private Player player;

    [SerializeField] private EnemySpawner enemySpawner;

    public List<Node> listOfNodes;

    void Start()
    {
        SpawnDungeon();
    }

    // Spawn a dungeon
    public void SpawnDungeon() 
    {
        // Destroy previous instance of dungeon
        // DestroyAllChildren();

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
        dungeonController.spawnRoom = listRooms.Find((room) => room.IsSpawn);
        dungeonController.exitRoom = listRooms.Find((room) => room.IsExit);

        List<CorridorNode> listCorridors = 
            listOfNodes.FindAll(node => node is CorridorNode)
            .Select(node => (CorridorNode) node).ToList();
        dungeonController.corridors = listCorridors;

        for (int i = 0; i < listRooms.Count; i++)
        {
            InstantiateRoom(listRooms[i], i, dungeonObject);
        }
        for (int i = 0; i < listCorridors.Count; i++)
        {
            InstantiateCorridor(listCorridors[i], i, dungeonObject);
        }
        
        NavMesh.BuildNavMesh();

        // Enemy spawns
        // good to have a list of enemies. Dunno what to do with them yet though
        dungeonController.rooms.ForEach((room) => {
            if(room.IsSpawn)
                return;
            room.SpawnEnemies(enemySpawner);
        });

        // Player spawn
        if (player != null) 
        { 
            Player p = Instantiate(player, new Vector3(dungeonController.spawnRoom.SpawnPoint.x, 0, dungeonController.spawnRoom.SpawnPoint.y), Quaternion.identity);
            p.CurrentDungeon = dungeonController;
            dungeonController.Player = p;
        }
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
            "Floor", 
            typeof(MeshFilter), 
            typeof(MeshRenderer));
        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = floorMaterial;
        dungeonFloor.transform.parent = parent.transform;

        BoxCollider collider = dungeonFloor.AddComponent<BoxCollider>();
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

    private void SpawnCeilingLight(RoomNode room, GameObject parent)
    {
        // Make a game object
        GameObject lightGameObject1 = new GameObject("Room Light 1");

        // Add the light component
        Light lightComp1 = lightGameObject1.AddComponent<Light>();

        lightComp1.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        lightComp1.intensity = 4;

        Vector2 center = room.Center;

        if (room.IsSpawn || room.IsExit || (room.Width < roomWidthMin && room.Length < roomLengthMin))
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

            lightComp2.color = new Color(0.8f, 0.8f, 0.8f, 1f);
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

    private void PopulateWithProps(RoomNode room)
    {
        Vector2 center = room.Center;

        // separate rooms into different types to spawn respective props
        if (room.IsSpawn)
        {
            room.Type = RoomType.SpawnRoom;
            PopulateSpawnRoom(room);
        }
        else if (room.IsExit)
        {
            room.Type = RoomType.ExitRoom;
            PopulateExitRoom(room);
        }
        else if (
            room.Width >= this.roomWidthMin * this.bigRoomMultiplier &&
            room.Length >= this.roomLengthMin * this.bigRoomMultiplier) // Big room
        {
            room.Type = RoomType.BeegRoom;
            PopulateBeegRoom(room);
            if (Random.Range(0,2) == 0) PopulateRoomCorners(room);
        }
        else if (
            room.Width >= 1.7 * room.Length &&
            room.ConnectedNodes.Exists(
                pair => 
                    (int) StructureHelper.GiveRelativePosition(center, pair.Item1.Center) >= 2)) // left/right position
        {
            room.Type = RoomType.Hallway;
            PopulateHallway(room, Orientation.Horizontal);
            if (Random.Range(0,2) == 0) PopulateRoomCorners(room);
        }
        else if (room.Length >= 1.7 * room.Width &&
            room.ConnectedNodes.Exists(
                pair => 
                    (int) StructureHelper.GiveRelativePosition(center, pair.Item1.Center) <= 1)) // up/down position
        {
            room.Type = RoomType.Hallway;
            PopulateHallway(room, Orientation.Vertical);
            if (Random.Range(0,2) == 0) PopulateRoomCorners(room);
        }
        else if (room.Width >= this.roomWidthMin && room.Length >= this.roomLengthMin)
        {
            room.Type = RoomType.ComputerRoom;
            PopulateComputerRoom(room);
            if (Random.Range(0,2) == 0) PopulateRoomCorners(room);
        }
        else if (
            room.ConnectedNodes.Count == 1 &&
            room.Width <= this.roomWidthMin * this.bigRoomMultiplier &&
            room.Length <= this.roomLengthMin * this.bigRoomMultiplier) 
        { // Dead End
            room.Type = RoomType.StorageRoom;
            PopulateStorageRoom(room);
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
                    PopulateLabRoom(room);
                    if (Random.Range(0,2) == 0) PopulateRoomCorners(room);
                    break;
                case (RoomType.Library):
                    PopulateLibrary(room);
                    break;
                default:
                    break;
            }
        }
    }

    private void PopulateHallway(RoomNode room, Orientation orientation)
    {
        if (orientation == Orientation.Horizontal)
        {
            float separation = room.Length / 3.0f;
            float row1 = room.BottomLeftAreaCorner.y + separation * 2;
            float row2 = room.BottomLeftAreaCorner.y + separation;

            separation = room.Width / ((float) Math.Ceiling(room.Width / (separation + 1)));
            for (
                float x = room.BottomLeftAreaCorner.x + separation; 
                x <= room.TopRightAreaCorner.x - separation + 0.1f; 
                x += separation)
            {
                room.Props.Add(new Prop(pillarObject, new Vector3(x, 0, row1)));
                room.Props.Add(new Prop(pillarObject, new Vector3(x, 0, row2)));
            }
        }
        else 
        {
            float separation = room.Width / 3.0f;
            float col1 = room.BottomLeftAreaCorner.x + separation * 2;
            float col2 = room.BottomLeftAreaCorner.x + separation;

            separation = room.Length / ((float) Math.Ceiling(room.Length / (separation + 1)));
            for (
                float y = room.BottomLeftAreaCorner.y + separation; 
                y <= room.TopRightAreaCorner.y - separation + 0.1f; 
                y += separation)
            {
                room.Props.Add(new Prop(pillarObject, new Vector3(col1, 0, y)));
                room.Props.Add(new Prop(pillarObject, new Vector3(col2, 0, y)));
            }
        }
    }

    private void PopulateStorageRoom(RoomNode room)
    {
        CorridorNode corridor = room.ConnectedNodes[0].Item1;
        RelativePosition entranceRelativeToRoom = 
            StructureHelper.GiveRelativePosition(room.Center, corridor.Center);
        
        int roulette = Random.Range(0, 2);

        // Rows of shelves
        if (roulette == 0)
        {
            float row1 = room.BottomLeftAreaCorner.y + room.Length * (3f/8f);
            float row2 = room.TopRightAreaCorner.y - room.Length * (3f/8f);

            int shelvesPerRow = room.Width / 4; // shelves take width of 2, and we only occupy half of the room width  
            float rowWidth = shelvesPerRow * 2 + 0.1f;
            float fromLeft = room.BottomLeftAreaCorner.x + ((room.Width - rowWidth) / 2.0f) + 1;
            for (int i = 0; i < shelvesPerRow; i++)
            {
                GameObject shelf = 
                    this.shelvesObjectsList[Random.Range(0, this.shelvesObjectsList.Length - 1)];
                room.Props.Add(
                    new Prop(
                        shelf,
                        new Vector3(fromLeft + i*2, 0, row1)));
                shelf = this.shelvesObjectsList[Random.Range(0, this.shelvesObjectsList.Length - 1)];
                room.Props.Add(
                    new Prop(
                        shelf,
                        new Vector3(fromLeft + i*2, 0, row2)));
            }
        } 
        else
        {
            float row1 = room.BottomLeftAreaCorner.x + room.Width * (3f/8f);
            float row2 = room.TopRightAreaCorner.x - room.Width * (3f/8f);

            int shelvesPerRow = room.Length / 4; // shelves take width of 2, and we only occupy half of the room width  
            float rowWidth = shelvesPerRow * 2 + 0.1f;
            float fromBottom = room.BottomLeftAreaCorner.y + ((room.Length - rowWidth) / 2.0f) + 1;
            for (int i = 0; i < shelvesPerRow; i++)
            {
                GameObject shelf = 
                    this.shelvesObjectsList[Random.Range(0, this.shelvesObjectsList.Length - 1)];
                room.Props.Add(
                    new Prop(
                        shelf,
                        new Vector3(row1, 0, fromBottom + i*2),
                        Quaternion.Euler(0f, 0f, 90f)));
                shelf = this.shelvesObjectsList[Random.Range(0, this.shelvesObjectsList.Length - 1)];
                room.Props.Add(
                    new Prop(
                        shelf,
                        new Vector3(row2, 0, fromBottom + i*2),
                        Quaternion.Euler(0f, 0f, 90f)));
            }
        }

    }

    private void PopulateSpawnRoom(RoomNode room)
    {
        room.Props.Add(
            new Prop(spawnFloorTile, new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y)));

        float floorTileLength = GetObjectBounds(spawnFloorTile).x;

        room.Props.Add(
            new Prop(
                spawnFloorTileEdge, 
                new Vector3(room.SpawnPoint.x - floorTileLength, 0, room.SpawnPoint.y), 
                Quaternion.Euler(0f,0f,180f)));
        room.Props.Add(
            new Prop(
                spawnFloorTileEdge, 
                new Vector3(room.SpawnPoint.x + floorTileLength, 0, room.SpawnPoint.y)));
        room.Props.Add(
            new Prop(
                spawnFloorTileEdge, 
                new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y - floorTileLength), 
                Quaternion.Euler(0f,0f,90f)));
        room.Props.Add(
            new Prop(
                spawnFloorTileEdge, 
                new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y + floorTileLength), 
                Quaternion.Euler(0f,0f,-90f)));

        room.Props.Add(
            new Prop(
                spawnFloorTileCorner, 
                new Vector3(
                    room.SpawnPoint.x - floorTileLength, 
                    0, 
                    room.SpawnPoint.y - floorTileLength), 
                Quaternion.Euler(0f,0f,90f)));
        room.Props.Add(
            new Prop(spawnFloorTileCorner, 
            new Vector3(
                room.SpawnPoint.x - floorTileLength, 
                0, 
                room.SpawnPoint.y + floorTileLength), 
            Quaternion.Euler(0f,0f,180f)));
        room.Props.Add(
            new Prop(
                spawnFloorTileCorner, 
                new Vector3(
                    room.SpawnPoint.x + floorTileLength, 
                    0, 
                    room.SpawnPoint.y - floorTileLength)));
        room.Props.Add(
            new Prop(
                spawnFloorTileCorner, 
                new Vector3(
                    room.SpawnPoint.x + floorTileLength, 
                    0, 
                    room.SpawnPoint.y + floorTileLength), 
                Quaternion.Euler(0f,0f,-90f)));
    }

    private void PopulateExitRoom(RoomNode room)
    {
        room.Props.Add(new Prop(exitObject, new Vector3(room.ExitPoint.x, 0, room.ExitPoint.y)));

        float xOffset = GetObjectBounds(torchObject).x / 2 + 0.1f;
        float zOffset = GetObjectBounds(torchObject).z / 2 + 0.1f;
        room.Props.Add(
            new Prop(
                torchObject, 
                new Vector3(
                    room.BottomLeftAreaCorner.x + xOffset, 
                    0, 
                    room.BottomLeftAreaCorner.y + zOffset)));
        room.Props.Add(
            new Prop(
                torchObject, 
                new Vector3(
                    room.BottomRightAreaCorner.x - xOffset, 
                    0, 
                    room.BottomRightAreaCorner.y + zOffset)));
        room.Props.Add(
            new Prop(
                torchObject, 
                new Vector3(
                    room.TopLeftAreaCorner.x + xOffset, 
                    0, 
                    room.TopLeftAreaCorner.y - zOffset)));
        room.Props.Add(
            new Prop(
                torchObject, 
                new Vector3(
                    room.TopRightAreaCorner.x - xOffset, 
                    0, 
                    room.TopRightAreaCorner.y - zOffset)));
    }

    private void PopulateBeegRoom(RoomNode room)
    {
        Vector2Int bottomLeft = room.BottomLeftAreaCorner;
        Vector2Int bottomRight = room.BottomRightAreaCorner;
        Vector2Int topLeft = room.TopLeftAreaCorner;
        Vector2Int topRight = room.TopRightAreaCorner;

        Vector2 center = StructureHelper.CalculateMiddlePoint(bottomLeft, topRight);
        Vector2 bottomLeftPillar = Vector2.Lerp(center, bottomLeft, 0.5f);
        Vector2 bottomRightPillar = Vector2.Lerp(center, bottomRight, 0.5f);
        Vector2 topLeftPillar = Vector2.Lerp(center, topLeft, 0.5f);
        Vector2 topRightPillar = Vector2.Lerp(center, topRight, 0.5f);

        room.Props.Add(
            new Prop(
                pillarObject, 
                new Vector3(bottomLeftPillar.x, 0, bottomLeftPillar.y)));
        room.Props.Add(
            new Prop(
                pillarObject, 
                new Vector3(bottomRightPillar.x, 0, bottomRightPillar.y)));
        room.Props.Add(
            new Prop(
                pillarObject, 
                new Vector3(topLeftPillar.x, 0, topLeftPillar.y)));
        room.Props.Add(
            new Prop(
                pillarObject, 
                new Vector3(topRightPillar.x, 0, topRightPillar.y)));

        room.Props.Add(new Prop(statueObject, new Vector3(room.Center.x, 0, room.Center.y)));
    }

    private void PopulateComputerRoom(RoomNode room)
    {
        Vector2 center = room.Center;
        float widthSpace = room.Width / 4f;
        float lengthSpace = room.Length / 4f;
        float computerPillarSeparationX = 
            GetObjectBounds(computerObject).x / 2f +
            GetObjectBounds(pillarObject).x / 2f;
        float computerPillarSeparationZ = 
            GetObjectBounds(computerObject).z / 2f +
            GetObjectBounds(pillarObject).z / 2f;

        room.Props.Add(new Prop(
            computerObject,
            new Vector3(center.x, 0, center.y + lengthSpace),
            Quaternion.Euler(0f,0f,180f)
        ));
        room.Props.Add(new Prop(
            spawnFloorTile,
            new Vector3(center.x, 0, center.y + lengthSpace)
        ));
        room.Props.Add(new Prop(
            pillarObject,
            new Vector3(center.x, 0, center.y + lengthSpace + computerPillarSeparationZ)
        ));

        room.Props.Add(new Prop(
            computerObject,
            new Vector3(center.x, 0, center.y - lengthSpace),
            Quaternion.Euler(0f,0f,0f)
        ));
        room.Props.Add(new Prop(
            spawnFloorTile,
            new Vector3(center.x, 0, center.y - lengthSpace)
        ));
        room.Props.Add(new Prop(
            pillarObject,
            new Vector3(center.x, 0, center.y - lengthSpace - computerPillarSeparationZ)
        ));

        room.Props.Add(new Prop(
            computerObject,
            new Vector3(center.x + widthSpace, 0, center.y),
            Quaternion.Euler(0f,0f,-90f)
        ));
        room.Props.Add(new Prop(
            spawnFloorTile,
            new Vector3(center.x + widthSpace, 0, center.y)
        ));
        room.Props.Add(new Prop(
            pillarObject,
            new Vector3(center.x + widthSpace + computerPillarSeparationX, 0, center.y)
        ));

        room.Props.Add(new Prop(
            computerObject,
            new Vector3(center.x - widthSpace, 0, center.y),
            Quaternion.Euler(0f,0f,90f)
        ));
        room.Props.Add(new Prop(
            spawnFloorTile,
            new Vector3(center.x - widthSpace, 0, center.y)
        ));
        room.Props.Add(new Prop(
            pillarObject,
            new Vector3(center.x - widthSpace - computerPillarSeparationX, 0, center.y)
        ));
    }

    private void PopulateLabRoom(RoomNode room)
    {
        Vector2 center = room.Center;

        GameObject labItem = labItemObjectList[Random.Range(0, labItemObjectList.Length)];
        RelativePosition computerPosition = (RelativePosition) Random.Range(0, 4);
        float computerLabItemSpace = 
            GetObjectBounds(computerObject).z / 2 +
            GetObjectBounds(labItem).z / 2 + 0.5f;

        if ((int) computerPosition >= 2) // left/right
        {
            room.Props.Add(new Prop(
                labItem, 
                new Vector3(center.x, 0, center.y),
                Quaternion.Euler(0f, 0f, 90f)));

            room.Props.Add(new Prop(
                computerObject,
                new Vector3(
                    center.x + (
                        computerPosition == RelativePosition.Left 
                        ? -computerLabItemSpace 
                        : computerLabItemSpace), 
                    0, 
                    center.y),
                computerPosition == RelativePosition.Left 
                ? Quaternion.Euler(0f, 0f, -90f) 
                : Quaternion.Euler(0f, 0f, 90f)
            ));
            
            room.Props.Add(new Prop(
                spawnFloorTile,
                new Vector3(
                    center.x + (
                        computerPosition == RelativePosition.Left 
                        ? -computerLabItemSpace 
                        : computerLabItemSpace), 
                    0, 
                    center.y)));
        }
        else // up/down
        {
            room.Props.Add(new Prop(
                labItem, 
                new Vector3(center.x, 0, center.y)));

            room.Props.Add(new Prop(
                computerObject,
                new Vector3(
                    center.x, 
                    0, 
                    center.y + (
                        computerPosition == RelativePosition.Down 
                        ? -computerLabItemSpace 
                        : computerLabItemSpace)),
                computerPosition == RelativePosition.Down 
                ? Quaternion.Euler(0f, 0f, 180f) 
                : Quaternion.Euler(0f, 0f, 0f)
            ));

            room.Props.Add(new Prop(
                spawnFloorTile,
                new Vector3(
                    center.x, 
                    0, 
                    center.y + (
                        computerPosition == RelativePosition.Down 
                        ? -computerLabItemSpace 
                        : computerLabItemSpace))));
        }

    }

    private void PopulateLibrary(RoomNode room)
    {
        int roulette = Random.Range(0,3);

        float shelfX = this.shelvesObjectsList.Select(shelf => GetObjectBounds(shelf).x).Max();
        float shelfZ = this.shelvesObjectsList.Select(shelf => GetObjectBounds(shelf).z).Max();
        float shelfOffset = shelfZ / 2f + 0.1f;

        GameObject computer = this.computerObject;
        GameObject tile = this.spawnFloorTile;
        float tileOffset = GetObjectBounds(tile).x / 2;
        int spaceBtwnComputers = 1;

        float from, row1, row2, noOfShelves;
        bool alignTopBottom;
        if (roulette == 1 || (roulette == 3 && room.Width > room.Length)) // Top/bottom
        {
            from = room.BottomLeftAreaCorner.x + shelfX / 2;
            if (room.Width % shelfX != 0) from += (room.Width % shelfX) / 2;
            row1 = room.TopRightAreaCorner.y - shelfOffset;
            row2 = room.BottomLeftAreaCorner.y + shelfOffset;
            noOfShelves = room.Width / shelfX - 1;
            alignTopBottom = true;
        }
        else
        {
            from = room.BottomLeftAreaCorner.y + shelfX / 2;
            if (room.Length % shelfX != 0) from += (room.Length % shelfX) / 2;
            row1 = room.TopRightAreaCorner.x - shelfOffset;
            row2 = room.BottomLeftAreaCorner.x + shelfOffset;
            noOfShelves = room.Length / shelfX - 1;
            alignTopBottom = false;
        }

        for (int i = 0; i < noOfShelves; i++)
        {
            Vector2 coord1 = 
                alignTopBottom
                ? new Vector2(from + i * shelfX, row1)
                : new Vector2(row1, from + i * shelfX);
            Vector2 coord2 = 
                alignTopBottom
                ? new Vector2(from + i * shelfX, row2)
                : new Vector2(row2, from + i * shelfX);

            if (
                (spaceBtwnComputers < 2 || i >= noOfShelves - 1 || Random.Range(0,2) >= 1) 
                && spaceBtwnComputers < 4)
            {
                spaceBtwnComputers++;

                GameObject shelf1 = 
                    this.shelvesObjectsList[Random.Range(0, this.shelvesObjectsList.Length - 1)];
                GameObject shelf2 = 
                    this.shelvesObjectsList[Random.Range(0, this.shelvesObjectsList.Length - 1)];

                Quaternion rotation = 
                    alignTopBottom 
                    ? Quaternion.identity 
                    : Quaternion.Euler(0f,0f,90f);
                if (CheckClearFromDoor(room, coord1, shelf1))
                    room.Props.Add(new Prop(shelf1, new Vector3(coord1.x, 0, coord1.y), rotation));
                if (CheckClearFromDoor(room, coord2, shelf2))
                    room.Props.Add(new Prop(shelf2, new Vector3(coord2.x, 0, coord2.y), rotation));
            }
            else
            {
                spaceBtwnComputers = 0;

                Vector2 tileCoord1 =
                    alignTopBottom
                    ? new Vector2(from + i * shelfX, row1 + shelfOffset - tileOffset)
                    : new Vector2(row1 + shelfOffset - tileOffset, from + i * shelfX);
                Vector2 tileCoord2 = 
                    alignTopBottom
                    ? new Vector2(from + i * shelfX, row2 - shelfOffset + tileOffset)
                    : new Vector2(row2 - shelfOffset + tileOffset, from + i * shelfX);
                
                if (CheckClearFromDoor(room, tileCoord1, tile))
                {
                    Quaternion rotation = 
                        alignTopBottom
                        ? Quaternion.Euler(0f,0f,180f)
                        : Quaternion.Euler(0f,0f,-90f);
                    room.Props.Add(new Prop(computer, new Vector3(coord1.x, 0, coord1.y), rotation));
                    room.Props.Add(new Prop(tile, new Vector3(tileCoord1.x, 0, tileCoord1.y)));
                }
                if (CheckClearFromDoor(room, tileCoord2, tile))
                {
                    Quaternion rotation = 
                        alignTopBottom
                        ? Quaternion.identity
                        : Quaternion.Euler(0f,0f,90f);
                    room.Props.Add(new Prop(computer, new Vector3(coord2.x, 0, coord2.y), rotation));
                    room.Props.Add(new Prop(tile, new Vector3(tileCoord2.x, 0, tileCoord2.y)));
                }
            }
        }
    }

    private void PopulateRoomCorners(RoomNode room)
    {
        // TODO: add more room types so we could replace the below
        int corner = Random.Range(0, 4);
        var prop = propsList[Random.Range(0, propsList.Length)];
        float xOffset = GetObjectBounds(prop).x / 2 + 0.1f;
        float zOffset = GetObjectBounds(prop).z / 2 + 0.1f;
        Vector2Int bottomLeft = room.BottomLeftAreaCorner;
        Vector2Int bottomRight = room.BottomRightAreaCorner;
        Vector2Int topLeft = room.TopLeftAreaCorner;
        Vector2Int topRight = room.TopRightAreaCorner;

        if (CheckClearFromDoor(room, bottomLeft, prop))
            room.Props.Add(new Prop(
                prop,
                new Vector3(bottomLeft.x + xOffset, 0, bottomLeft.y + zOffset)));
        if (CheckClearFromDoor(room, bottomRight, prop))
            room.Props.Add(new Prop(
                prop,
                new Vector3(bottomRight.x - xOffset, 0, bottomRight.y + zOffset)));
        if (CheckClearFromDoor(room, topLeft, prop))
            room.Props.Add(new Prop(
                prop,
                new Vector3(topLeft.x + xOffset, 0, topLeft.y - zOffset)));
        if (CheckClearFromDoor(room, topRight, prop))
            room.Props.Add(new Prop(
                prop,
                new Vector3(topRight.x - xOffset, 0, topRight.y - zOffset)));
    }
    // Get GameObject's bound size 
    private Vector3 GetObjectBounds(GameObject obj)
    {
        return obj.GetComponent<Renderer>().bounds.size;
    }

    // Check if a coordinate in a room is free from any doorways such that
    // a prop there will not obstruct it
    private bool CheckClearFromDoor(RoomNode room, Vector2 coord, GameObject prop)
    {
        Vector3 propBounds = GetObjectBounds(prop);
        float range = 
            (float) (Math.Sqrt(Math.Pow(propBounds.x / 2, 2) + Math.Pow(propBounds.z / 2, 2))) + 1f;

        return room.Doors.All(door => 
            door.orientation == Orientation.Horizontal 
                ? Vector2.Distance(coord, door.coordinates + new Vector2(0.5f,0f)) > range 
                : Vector2.Distance(coord, door.coordinates + new Vector2(0f,0.5f)) > range);
    }

    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }

}
