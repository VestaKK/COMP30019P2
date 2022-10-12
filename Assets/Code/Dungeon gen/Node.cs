using System.Collections.Generic;
using UnityEngine;

// Abstract class representing a node in a tree
public abstract class Node
{
    private List<Node> childrenNodeList;

    public List<Node> ChildrenNodeList
    {
        get => childrenNodeList;
    }

    public bool Visited { get; set; }

    // Coordinates for corners (for rooms & corridors)
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }

    public Node Parent { get; set; }
    
    // Depth index in tree
    public int TreeLayerIndex { get; set; }

    public Node(Node parentNode)
    {
        childrenNodeList = new List<Node>();
        this.Parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
        childrenNodeList.Add(node);
    }

    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node);
    }
}