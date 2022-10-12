using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonGenerator
{
    
    List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int dungeonWidth;
    private int dungeonLength;

    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }

    public List<Node> CalculateRoomsAndCorridors(
        int maxIterations, 
        int roomWidthMin, 
        int roomLengthMin, 
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
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomWidthMin, roomLengthMin);
        List<RoomNode> roomList = 
            roomGenerator.GenerateRoomsInGivenSpaces(
                roomSpaces, 
                bottomCornerModifier, 
                topCornerModifier, 
                offset);

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