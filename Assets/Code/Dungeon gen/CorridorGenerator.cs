using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorGenerator
{
    // Create Corridors given a list of rooms
    public List<Node> CreateCorridors(
        List<RoomNode> allNodesCollection, 
        int corridorWidth, 
        int distanceFromWall)
    {
        List<Node> corridorList = new List<Node>();
        // sort queue of nodes based on tree depth, leaf nodes coming first
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allNodesCollection.OrderByDescending(node => node.TreeLayerIndex).ToList());

        while (structuresToCheck.Count > 0)
        {
            var node = structuresToCheck.Dequeue();
            // ignore node with no children 
            if (node.ChildrenNodeList.Count == 0)
                continue;

            // Generate corridor between the children of this node
            CorridorNode corridor = new CorridorNode(
                node.ChildrenNodeList[0], 
                node.ChildrenNodeList[1], 
                corridorWidth, 
                distanceFromWall);
            corridorList.Add(corridor);
        }

        return corridorList;
    }
}