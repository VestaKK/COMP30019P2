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

    // Game object for walls
    [SerializeField] GameObject wallObject;

    [SerializeField] private GameObject spawnObject;
    [SerializeField] private GameObject spawnFloorTile, spawnFloorTileEdge, spawnFloorTileCorner;
    [SerializeField] private GameObject exitObject;
    [SerializeField] private GameObject[] shelvesObjectsList;
    [SerializeField] private GameObject torchObject;
    [SerializeField] private GameObject[] propsList;
    [SerializeField] private int propsPerRoomMin, propsPerRoomMax;
    [SerializeField] private float bigRoomMultiplier;
    [SerializeField] private GameObject pillarObject;

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

        // Populate room with props
        List<RoomNode> listRooms = 
            listOfNodes.FindAll(node => node is RoomNode)
            .Select(node => (RoomNode) node).ToList();
        foreach (RoomNode room in listRooms)
        {
            PopulateWithProps(room);
        }

        // DEBUG purposes
        // List<RoomNode> listRooms = 
        //     listOfNodes.FindAll(node => node is RoomNode)
        //     .Select(node => (RoomNode) node).ToList();
        // spawnRoom = listRooms.Find((room) => room.IsSpawn);
        // Debug.Log(spawnRoom.SpawnPoint);
        // Debug.Log(spawnRoom.ConnectedNodes.Count);
        // exitRoom = listRooms.Find((room) => room.IsExit);
        // Debug.Log(exitRoom.ExitPoint);

        // Create Dungeon parent object
        GameObject dungeonObject = new GameObject("Dungeon");
        DungeonController dungeonController = dungeonObject.AddComponent<DungeonController>();
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
            // BoxCollider collider = newProp.AddComponent<BoxCollider>();
            // newProp.transform.position += 
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
        GameObject lightGameObject = new GameObject("Room Light");

        // Add the light component
        Light lightComp = lightGameObject.AddComponent<Light>();

        lightComp.color = new Color(0.4528302f, 0.04205423f, 0f, 1f);
        lightComp.intensity = 4;

        // Set the position (or any transform property)
        Vector2 center =  
            StructureHelper.CalculateCenter(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
        lightGameObject.transform.position = new Vector3(center.x, 4, center.y);
        lightGameObject.transform.parent = parent.transform;
    }

    private void PopulateWithProps(RoomNode room)
    {
        Vector2Int bottomLeft = room.BottomLeftAreaCorner;
        Vector2Int bottomRight = room.BottomRightAreaCorner;
        Vector2Int topLeft = room.TopLeftAreaCorner;
        Vector2Int topRight = room.TopRightAreaCorner;

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
        }
        else if (room.Width > 1.8 * room.Length)
        { 

        }
        else if (room.Length > 1.8 * room.Width)
        {

        }
        else
        {
            int corner = Random.Range(0,4);
            var prop = propsList[Random.Range(0, propsList.Length)];
            float xOffset = prop.GetComponent<Renderer>().bounds.size.x / 2 + 0.1f;
            float zOffset = prop.GetComponent<Renderer>().bounds.size.z / 2 + 0.1f;
            switch (corner)
            {
                case 0: // bottom left
                    // Instantiate(prop, new Vector3(bottomLeft.x + xOffset, yOffset, bottomLeft.y + zOffset), Quaternion.identity, parent.transform);
                    room.Props.Add(
                        new Prop(
                            prop, 
                            new Vector3(bottomLeft.x + xOffset, 0, bottomLeft.y + zOffset)));
                    break;
                case 1: // bottom right
                    // Instantiate(prop, new Vector3(topRight.x - xOffset, yOffset, bottomLeft.y + zOffset), Quaternion.identity, parent.transform);
                    room.Props.Add(
                        new Prop(
                            prop, 
                            new Vector3(topRight.x - xOffset, 0, bottomLeft.y + zOffset)));
                    break;
                case 2: // top left
                    // Instantiate(prop, new Vector3(bottomLeft.x + xOffset, yOffset, topRight.y - zOffset), Quaternion.identity, parent.transform);
                    room.Props.Add(
                        new Prop(
                            prop, 
                            new Vector3(bottomLeft.x + xOffset, 0, topRight.y - zOffset)));
                    break;
                case 3: // top right
                    // Instantiate(prop, new Vector3(topRight.x - xOffset, yOffset, topRight.y - zOffset), Quaternion.identity, parent.transform);
                    room.Props.Add(
                        new Prop(
                            prop, 
                            new Vector3(topRight.x - xOffset, 0, topRight.y - zOffset)));
                    break;
                default:
                    break;
            }
        }
    }

    public void PopulateSpawnRoom(RoomNode room)
    {
        // room.Props.Add(
        //     new Prop(
        //         spawnObject, 
        //         new Vector3(
        //             room.SpawnPoint.x, 
        //             0,
        //             room.SpawnPoint.y)));
        

        room.Props.Add(new Prop(spawnFloorTile, new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y)));

        float floorTileLength = spawnFloorTile.GetComponent<Renderer>().bounds.size.x;

        room.Props.Add(new Prop(spawnFloorTileEdge, new Vector3(room.SpawnPoint.x - floorTileLength, 0, room.SpawnPoint.y), Quaternion.Euler(0f,0f,180f)));
        room.Props.Add(new Prop(spawnFloorTileEdge, new Vector3(room.SpawnPoint.x + floorTileLength, 0, room.SpawnPoint.y)));
        room.Props.Add(new Prop(spawnFloorTileEdge, new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y - floorTileLength), Quaternion.Euler(0f,0f,90f)));
        room.Props.Add(new Prop(spawnFloorTileEdge, new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y + floorTileLength), Quaternion.Euler(0f,0f,-90f)));

        room.Props.Add(new Prop(spawnFloorTileCorner, new Vector3(room.SpawnPoint.x - floorTileLength, 0, room.SpawnPoint.y - floorTileLength), Quaternion.Euler(0f,0f,90f)));
        room.Props.Add(new Prop(spawnFloorTileCorner, new Vector3(room.SpawnPoint.x - floorTileLength, 0, room.SpawnPoint.y + floorTileLength), Quaternion.Euler(0f,0f,180f)));
        room.Props.Add(new Prop(spawnFloorTileCorner, new Vector3(room.SpawnPoint.x + floorTileLength, 0, room.SpawnPoint.y - floorTileLength)));
        room.Props.Add(new Prop(spawnFloorTileCorner, new Vector3(room.SpawnPoint.x + floorTileLength, 0, room.SpawnPoint.y + floorTileLength), Quaternion.Euler(0f,0f,-90f)));
    }

    public void PopulateExitRoom(RoomNode room)
    {
        room.Props.Add(new Prop(exitObject, new Vector3(room.ExitPoint.x, 0, room.ExitPoint.y)));

        float xOffset = torchObject.GetComponent<Renderer>().bounds.size.x / 2 + 0.1f;
        float zOffset = torchObject.GetComponent<Renderer>().bounds.size.z / 2 + 0.1f;
        room.Props.Add(new Prop(torchObject, new Vector3(room.BottomLeftAreaCorner.x + xOffset, 0, room.BottomLeftAreaCorner.y + zOffset)));
        room.Props.Add(new Prop(torchObject, new Vector3(room.BottomRightAreaCorner.x - xOffset, 0, room.BottomRightAreaCorner.y + zOffset)));
        room.Props.Add(new Prop(torchObject, new Vector3(room.TopLeftAreaCorner.x + xOffset, 0, room.TopLeftAreaCorner.y - zOffset)));
        room.Props.Add(new Prop(torchObject, new Vector3(room.TopRightAreaCorner.x - xOffset, 0, room.TopRightAreaCorner.y - zOffset)));
    }

    public void PopulateBeegRoom(RoomNode room)
    {
        Vector2Int bottomLeft = room.BottomLeftAreaCorner;
        Vector2Int bottomRight = room.BottomRightAreaCorner;
        Vector2Int topLeft = room.TopLeftAreaCorner;
        Vector2Int topRight = room.TopRightAreaCorner;

        Vector2 center = StructureHelper.CalculateMiddlePoint(bottomLeft, topRight);
        Vector2 bottomLeftPillar = Vector2.Lerp(center, bottomLeft, 0.3f);
        Vector2 bottomRightPillar = Vector2.Lerp(center, bottomRight, 0.3f);
        Vector2 topLeftPillar = Vector2.Lerp(center, topLeft, 0.3f);
        Vector2 topRightPillar = Vector2.Lerp(center, topRight, 0.3f);

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
