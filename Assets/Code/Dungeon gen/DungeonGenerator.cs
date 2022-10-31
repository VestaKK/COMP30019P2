using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public class DungeonGenerator
{
    
    List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int dungeonWidth;
    private int dungeonLength;

    private DungeonController dungeonController;

    public DungeonGenerator(DungeonController dungeonController, int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
        this.dungeonController = dungeonController;
    }

    public List<Node> CalculateRoomsAndCorridors(
        int maxIterations, 
        int roomWidthMin, 
        int roomLengthMin, 
        int spawnRoomWidth,
        int spawnRoomLength,
        int exitRoomWidth,
        int exitRoomLength,
        float bottomCornerModifier, 
        float topCornerModifier, 
        int offset, 
        int corridorWidth, 
        int distanceFromWall)
    {
        // Partition dungeon and get all room spaces from tree
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonWidth);
        allNodesCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaves(bsp.RootNode);

        // Generate rooms from the room spaces
        RoomGenerator roomGenerator = new RoomGenerator(dungeonController, maxIterations);
        List<RoomNode> roomList = 
            roomGenerator.GenerateRoomsInGivenSpaces(
                roomSpaces, 
                bottomCornerModifier, 
                topCornerModifier, 
                offset);

        // Find room center closest to center of dungeon, set as spawn
        RoomNode spawnRoom = 
            roomList.OrderBy(
                child => Vector2.Distance(
                    child.MiddlePoint, new Vector2(dungeonWidth/2, dungeonLength/2))
            ).ToList()[0];
        spawnRoom.Type = RoomType.SpawnRoom;

        // Adjust spawn room size
        float spawnWidthChange, spawnLengthChange;
        spawnWidthChange = (spawnRoom.Width - spawnRoomWidth) / 2;
        spawnLengthChange = (spawnRoom.Length - spawnRoomLength) / 2;
        spawnRoom.BottomLeftAreaCorner += 
            new Vector2Int(Mathf.FloorToInt(spawnWidthChange), Mathf.FloorToInt(spawnLengthChange));
        spawnRoom.BottomRightAreaCorner += 
            new Vector2Int(-Mathf.FloorToInt(spawnWidthChange), Mathf.CeilToInt(spawnLengthChange));
        spawnRoom.TopLeftAreaCorner += 
            new Vector2Int(Mathf.CeilToInt(spawnWidthChange), -Mathf.FloorToInt(spawnLengthChange));
        spawnRoom.TopRightAreaCorner += 
            new Vector2Int(-Mathf.CeilToInt(spawnWidthChange), -Mathf.CeilToInt(spawnLengthChange));
        spawnRoom.SpawnPoint = spawnRoom.Center;
        spawnRoom.GenerateWalls();

        // Randomly select exit room between four corners
        RoomNode exitRoom;
        int corner = Random.Range(0,4);
        if (corner == 0) // Bottom left corner exit
        {   
            exitRoom = 
                roomList.OrderBy(
                    child => child.BottomLeftAreaCorner.x + child.BottomLeftAreaCorner.y
                ).ToList()[0];
        }
        else if (corner == 1) // Bottom Right corner exit
        {
            exitRoom = 
                roomList.OrderBy(
                    child => - child.BottomLeftAreaCorner.x + child.BottomLeftAreaCorner.y
                ).ToList()[0];
        }
        else if (corner == 2) // Top Left corner exit
        {
            exitRoom = 
                roomList.OrderBy(
                    child => child.BottomLeftAreaCorner.x - child.BottomLeftAreaCorner.y
                ).ToList()[0];
        }
        else // Top Right corner exit
        {
            exitRoom = 
                roomList.OrderByDescending(
                    child => child.BottomLeftAreaCorner.x + child.BottomLeftAreaCorner.y
                ).ToList()[0];
        }
        exitRoom.Type = RoomType.ExitRoom;

        // Adjust exit room size
        float exitWidthChange, exitLengthChange;
        exitWidthChange = (exitRoom.Width - exitRoomWidth) / 2;
        exitLengthChange = (exitRoom.Length - exitRoomLength) / 2;
        exitRoom.BottomLeftAreaCorner += 
            new Vector2Int(Mathf.FloorToInt(exitWidthChange), Mathf.FloorToInt(exitLengthChange));
        exitRoom.BottomRightAreaCorner += 
            new Vector2Int(-Mathf.FloorToInt(exitWidthChange), Mathf.CeilToInt(exitLengthChange));
        exitRoom.TopLeftAreaCorner += 
            new Vector2Int(Mathf.CeilToInt(exitWidthChange), -Mathf.FloorToInt(exitLengthChange));
        exitRoom.TopRightAreaCorner += 
            new Vector2Int(-Mathf.CeilToInt(exitWidthChange), -Mathf.CeilToInt(exitLengthChange));
        exitRoom.ExitPoint = exitRoom.Center;
        exitRoom.GenerateWalls();

        // Generate corridors given the rooms we generated
        CorridorGenerator corridorGenerator = new CorridorGenerator();
        var corridorsList = 
            corridorGenerator.CreateCorridors(
                allNodesCollection, 
                corridorWidth, 
                distanceFromWall);

        return new List<Node>(roomList).Concat(corridorsList).ToList();
    }
}