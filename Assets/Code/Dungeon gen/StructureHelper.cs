
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Helper class storing functions for specific tasks
public static class StructureHelper
{
    // Traverse a tree/graph and return only the leaves of it
    public static List<Node> TraverseGraphToExtractLowestLeaves(Node parentNode)
    { 
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();

        if (parentNode.ChildrenNodeList.Count == 0)
            return new List<Node>() {parentNode}; // return itself if parentNode is a leaf

        foreach(var child in parentNode.ChildrenNodeList)
        {
            nodesToCheck.Enqueue(child);
        }
        while (nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if (currentNode.ChildrenNodeList.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach (var child in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }

        return listToReturn;
    }

    // Generate a random bottom left coordinate
    public static Vector2Int GenerateBottomLeftCornerBetween(
        Vector2Int boundaryLeftPoint, 
        Vector2Int boundaryRightPoint, 
        float pointModifier, 
        int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range(minX, (int) (minX + (maxX - minX) * pointModifier)), 
            Random.Range(minY, (int) (minY + (maxY - minY) * pointModifier)));
    }

    // Generate a random top right coordinate
    public static Vector2Int GenerateTopRightCornerBetween(
        Vector2Int boundaryLeftPoint, 
        Vector2Int boundaryRightPoint, 
        float pointModifier, 
        int offset )
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range((int) (minX + (maxX - minX) * pointModifier), maxX), 
            Random.Range((int) (minY + (maxY - minY) * pointModifier), maxY));
    }

    // Find middle point between two points
    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tmp = sum / 2;
        return new Vector2Int((int) tmp.x, (int) tmp.y);
    }

    // Give relative position of v2 according to v1
    public static RelativePosition GiveRelativePosition(Vector2 v1, Vector2 v2)
    {
        float angle = 
            Mathf.Atan2(
                v2.y - v1.y, 
                v2.x - v1.x) * Mathf.Rad2Deg;

        if ((angle < 45 && angle >= 0) || (angle > -45 && angle <= 0))
            return RelativePosition.Right;
        else if (angle > 45 && angle < 135)
            return RelativePosition.Up;
        else if (angle > -135 && angle < -45)
            return RelativePosition.Down;
        else 
            return RelativePosition.Left;
    }

}

public enum RelativePosition
{
    Up = 0, Down = 1, Left = 2, Right = 3
}