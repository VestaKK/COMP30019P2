using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


public class CorridorNode : Node
{
    // The two structure nodes this corridor is conencted to 
    private Node structure1;
    private Node structure2;

    // Corridor width
    private int corridorWidth;
    // Corridor distance from any wall
    private int distanceFromWall;

    // The two rooms this corridor is connected to
    private RoomNode room1, room2;
    public RoomNode Room1 { get => room1; }
    public RoomNode Room2 { get => room2; }

    public CorridorNode(
        Node structure1, 
        Node structure2,
        int corridorWidth, 
        int distanceFromWall) : base(null)
    {
        this.structure1 = structure1;
        this.structure2 = structure2;
        this.corridorWidth = corridorWidth;
        this.distanceFromWall = distanceFromWall;

        GenerateCorridor();
    }

    private void GenerateCorridor()
    {
        // Depending on where one node is relative to the other, 
        // process corridor accordingly
        var relativePositionOfStructure2 = CheckPositionStructure2AgainstStructure1();
        switch (relativePositionOfStructure2)
        {
            case RelativePosition.Up:
                ProcessRoomInRelationUpOrDown(this.structure1, this.structure2);
                break;
            case RelativePosition.Down:
                ProcessRoomInRelationUpOrDown(this.structure2, this.structure1);
                break;
            case RelativePosition.Right:
                ProcessRoomInRelationRightOrLeft(this.structure1, this.structure2);
                break;
            case RelativePosition.Left:
                ProcessRoomInRelationRightOrLeft(this.structure2, this.structure1);
                break;
            default:
                break;
        }

        // Connect rooms and corridors
        this.room1.ConnectedNodes.Add((this, this.room2));
        this.room2.ConnectedNodes.Add((this, this.room1));

        // Add walls
        for (int row = this.BottomLeftAreaCorner.x; row < this.BottomRightAreaCorner.x; row++)
        {
            Vector2Int wallPosition = new Vector2Int(row, this.BottomLeftAreaCorner.y);
            AddWall(Orientation.Horizontal, wallPosition);
        }
        for (int row = this.TopLeftAreaCorner.x; row < this.TopRightAreaCorner.x; row++)
        {
            Vector2Int wallPosition = new Vector2Int(row, this.TopLeftAreaCorner.y);
            AddWall(Orientation.Horizontal, wallPosition);
        }
        for (int col = this.BottomLeftAreaCorner.y; col < this.TopLeftAreaCorner.y; col++)
        {
            Vector2Int wallPosition = new Vector2Int(this.BottomLeftAreaCorner.x, col);
            AddWall(Orientation.Vertical, wallPosition);
        }
        for (int col = this.BottomRightAreaCorner.y; col < this.TopRightAreaCorner.y; col++)
        {
            Vector2Int wallPosition = new Vector2Int(this.BottomRightAreaCorner.x, col);
            AddWall(Orientation.Vertical, wallPosition);
        }
    }

    // Check where structure2 is relative to structure1, returning a cardinal direction
    // using *** MATH ****
    private RelativePosition CheckPositionStructure2AgainstStructure1()
    {
        Vector2 middlePointStructure1Tmp 
            = ((Vector2) structure1.TopRightAreaCorner + structure1.BottomLeftAreaCorner) / 2;
        Vector2 middlePointStructure2Tmp 
            = ((Vector2) structure2.TopRightAreaCorner + structure2.BottomLeftAreaCorner) / 2;

        return StructureHelper.GiveRelativePosition(middlePointStructure1Tmp, middlePointStructure2Tmp);
    }

    // Figure out how to generate a corridor between the two structures
    // knowing that the two structures are above one another
    private void ProcessRoomInRelationUpOrDown(Node structure1, Node structure2)
    {
        Node bottomStructure = null;
        List<Node> structureBottomChildren = 
            StructureHelper.TraverseGraphToExtractLowestLeaves(structure1);

        Node topStructure = null;
        List<Node> structureAboveChildren = 
            StructureHelper.TraverseGraphToExtractLowestLeaves(structure2);

        var sortedBottomStructure = 
            structureBottomChildren.OrderByDescending(child => child.TopRightAreaCorner.y).ToList();

        if (sortedBottomStructure.Count == 1) // if structure1 is a leaf node
        {
            bottomStructure = structureBottomChildren[0];
        }
        else
        {
            // select leaf node under structure1 that are amongst the 
            // furthest top side at random
            int maxY = sortedBottomStructure[0].TopLeftAreaCorner.y;
            sortedBottomStructure = 
                sortedBottomStructure.Where(
                    child => Mathf.Abs(maxY - child.TopLeftAreaCorner.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedBottomStructure.Count);
            bottomStructure = sortedBottomStructure[index];
        }

        // Get all structures within top structure that can
        // connect to current bottom structure
        var possibleNeighboursInTopStructure = 
            structureAboveChildren.Where(
                child => GetValidXForNeighbourUpDown(
                    bottomStructure.TopLeftAreaCorner,
                    bottomStructure.TopRightAreaCorner,
                    child.BottomLeftAreaCorner,
                    child.BottomRightAreaCorner) != -1)
            .OrderBy(child => child.BottomRightAreaCorner.y).ToList();

        if (possibleNeighboursInTopStructure.Count == 0)
            topStructure = structure2;
        else
            topStructure = possibleNeighboursInTopStructure[0];

        int x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner);

        // Keep looking for bottom structure that can connect to a top structure
        while(x == -1 && sortedBottomStructure.Count > 1)
        {
            sortedBottomStructure = 
                sortedBottomStructure.Where(
                    child => child.TopLeftAreaCorner.x != topStructure.TopLeftAreaCorner.x).ToList();
            bottomStructure = sortedBottomStructure[0];
            x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner);
        }

        this.BottomLeftAreaCorner = 
            new Vector2Int(x, bottomStructure.TopLeftAreaCorner.y);
        this.TopRightAreaCorner = 
            new Vector2Int(x + this.corridorWidth, topStructure.BottomLeftAreaCorner.y);
        this.BottomRightAreaCorner = 
            new Vector2Int(this.TopRightAreaCorner.x, this.BottomLeftAreaCorner.y);
        this.TopLeftAreaCorner =
            new Vector2Int(this.BottomLeftAreaCorner.x, this.TopRightAreaCorner.y);

        this.room1 = (RoomNode) bottomStructure;
        this.room2 = (RoomNode) topStructure;
    }

    // get a x coordinate for a corridor that will be valid between two rooms, -1 if cannot be found
    private int GetValidXForNeighbourUpDown(
        Vector2Int bottomNodeLeft, 
        Vector2Int bottomNodeRight, 
        Vector2Int topNodeLeft, 
        Vector2Int topNodeRight)
    {
        // if bottom node narrower than top node
        if (topNodeLeft.x < bottomNodeLeft.x && bottomNodeRight.x < topNodeRight.x)
            return StructureHelper.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(distanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.corridorWidth + distanceFromWall, 0)).x;
        
        // if bottom node narrower than top node
        else if (topNodeLeft.x >= bottomNodeLeft.x && bottomNodeRight.x >= topNodeRight.x)
            return StructureHelper.CalculateMiddlePoint(
                topNodeLeft+new Vector2Int(distanceFromWall,0),
                topNodeRight - new Vector2Int(this.corridorWidth + distanceFromWall,0)).x;
        
        // if bottom node more right than top node (but not completely to the right)
        else if (bottomNodeLeft.x >= topNodeLeft.x && bottomNodeLeft.x <= topNodeRight.x)
            return StructureHelper.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(distanceFromWall,0),
                topNodeRight - new Vector2Int(this.corridorWidth + distanceFromWall,0)).x;
        
        // if bottom node more left than top node (but not completely to the left)
        else if (bottomNodeRight.x <= topNodeRight.x && bottomNodeRight.x >= topNodeLeft.x)
            return StructureHelper.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(distanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.corridorWidth + distanceFromWall, 0)).x;
        
        else return -1; // no valid connection between nodes
    }

    // Figure out how to generate a corridor between the two structures
    // knowing that the two structures are beside each other horizontally
    private void ProcessRoomInRelationRightOrLeft(Node structure1, Node structure2)
    {
        Node leftStructure = null;
        List<Node> leftStructureChildren = 
            StructureHelper.TraverseGraphToExtractLowestLeaves(structure1);

        Node rightStructure = null;
        List<Node> rightStructureChildren = 
            StructureHelper.TraverseGraphToExtractLowestLeaves(structure2);

        var sortedLeftStructure = 
            leftStructureChildren.OrderByDescending(child => child.TopRightAreaCorner.x).ToList();

        if (sortedLeftStructure.Count == 1) // if structure1 is a leaf node
        {
            leftStructure = sortedLeftStructure[0];
        }
        else
        {
            // select leaf node under structure1 that are amongst the 
            // furthest right side at random
            int maxX = sortedLeftStructure[0].TopRightAreaCorner.x;
            sortedLeftStructure = 
                sortedLeftStructure.Where(
                    child => Math.Abs(maxX - child.TopRightAreaCorner.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedLeftStructure.Count);
            leftStructure = sortedLeftStructure[index];
        }

        // Get all structures within right structure that can
        // connect to current left structure
        var possibleNeighboursInRightStructureList = 
            rightStructureChildren.Where(
                child => GetValidYForNeighbourLeftRight(
                    leftStructure.TopRightAreaCorner, 
                    leftStructure.BottomRightAreaCorner, 
                    child.TopLeftAreaCorner, 
                    child.BottomLeftAreaCorner) != -1)
            .OrderBy(child => child.BottomRightAreaCorner.x).ToList();
        
        if (possibleNeighboursInRightStructureList.Count <= 0)
            rightStructure = structure2;
        else 
            rightStructure = possibleNeighboursInRightStructureList[0];
        
        int y = GetValidYForNeighbourLeftRight(
            leftStructure.TopLeftAreaCorner, 
            leftStructure.BottomLeftAreaCorner, 
            rightStructure.TopLeftAreaCorner, 
            rightStructure.BottomLeftAreaCorner);

        // Keep looking for left structure that can connect to a right structure
        while (y == -1 && sortedLeftStructure.Count > 1) 
        {
            sortedLeftStructure = 
                sortedLeftStructure.Where(
                    child => child.TopLeftAreaCorner.y != leftStructure.TopLeftAreaCorner.y).ToList();
            leftStructure = sortedLeftStructure[0];
            y = GetValidYForNeighbourLeftRight(
                leftStructure.TopLeftAreaCorner, 
                leftStructure.BottomLeftAreaCorner, 
                rightStructure.TopLeftAreaCorner, 
                rightStructure.BottomLeftAreaCorner);
        }

        this.BottomLeftAreaCorner = 
            new Vector2Int(leftStructure.BottomRightAreaCorner.x, y);
        this.TopRightAreaCorner = new Vector2Int(
            rightStructure.TopLeftAreaCorner.x, 
            y + this.corridorWidth);
        this.BottomRightAreaCorner = 
            new Vector2Int(this.TopRightAreaCorner.x, this.BottomLeftAreaCorner.y);
        this.TopLeftAreaCorner = 
            new Vector2Int(this.BottomLeftAreaCorner.x, this.TopRightAreaCorner.y);

        this.room1 = (RoomNode) leftStructure;
        this.room2 = (RoomNode) rightStructure;
    }

    // get a y coordinate for a corridor that will be valid between two rooms, -1 if cannot be found
    private int GetValidYForNeighbourLeftRight(
        Vector2Int leftNodeUp, 
        Vector2Int leftNodeDown, 
        Vector2Int rightNodeUp, 
        Vector2Int rightNodeDown)
    {
        // if left node shorter than right node
        if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
            return StructureHelper.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, distanceFromWall), 
                leftNodeUp - new Vector2Int(0, distanceFromWall + this.corridorWidth)).y;

        // if left node taller than right node
        else if (rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
            return StructureHelper.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, distanceFromWall), 
                rightNodeUp - new Vector2Int(0, distanceFromWall + this.corridorWidth)).y;

        // if left node higher than right node (but not completely above)
        else if (leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y)
            return StructureHelper.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, distanceFromWall), 
                leftNodeUp - new Vector2Int(0, distanceFromWall + this.corridorWidth)).y;

        // if left node lower than right node (but not completely below)
        else if (leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
            return StructureHelper.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, distanceFromWall), 
                rightNodeUp - new Vector2Int(0, distanceFromWall + this.corridorWidth)).y;

        else return -1; // no valid connection between nodes
    }

    // Either add wall to corridor or remove a wall from neighbouring room
    // to open an entrance
    private void AddWall(Orientation orientation, Vector2Int wallPosition)
    {
        Wall dupe1 = room1.Walls.Find(
            wall => wall.orientation == orientation && wall.coordinates == wallPosition);
        Wall dupe2 = room2.Walls.Find(
            wall => wall.orientation == orientation && wall.coordinates == wallPosition);
        if (dupe1 != null)
        {
            room1.Walls.Remove(dupe1);
            room1.Doors.Add(new Door(orientation, wallPosition));
        }
        else if (dupe2 != null)
        {
            room2.Walls.Remove(dupe2);
            room2.Doors.Add(new Door(orientation, wallPosition));
        }
        else 
            this.Walls.Add(new Wall(orientation, wallPosition));
    }
}