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
    
    public List<Wall> Walls { get; set; }

    public Node Parent { get; set; }

    /**
        Bounds from top left of each node
    */
    private Rect _spawnBounds;
    
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
        Walls = new List<Wall>();
    }

    public void AddChild(Node node)
    {
        childrenNodeList.Add(node);
    }

    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node);
    }

    public Vector2 Center { get => (TopRightAreaCorner + BottomLeftAreaCorner) / 2; }

    public void GenerateWalls()
    {
        this.Walls = new List<Wall>();
        for (int row = this.BottomLeftAreaCorner.x; row < this.BottomRightAreaCorner.x; row++)
        {
            Vector2Int wallPosition = new Vector2Int(row, this.BottomLeftAreaCorner.y);
            this.Walls.Add(new Wall(Orientation.Horizontal, wallPosition));
        }
        for (int row = this.TopLeftAreaCorner.x; row < this.TopRightAreaCorner.x; row++)
        {
            Vector2Int wallPosition = new Vector2Int(row, this.TopLeftAreaCorner.y);
            this.Walls.Add(new Wall(Orientation.Horizontal, wallPosition));
        }
        for (int col = this.BottomLeftAreaCorner.y; col < this.TopLeftAreaCorner.y; col++)
        {
            Vector2Int wallPosition = new Vector2Int(this.BottomLeftAreaCorner.x, col);
            this.Walls.Add(new Wall(Orientation.Vertical, wallPosition));
        }
        for (int col = this.BottomRightAreaCorner.y; col < this.TopRightAreaCorner.y; col++)
        {
            Vector2Int wallPosition = new Vector2Int(this.BottomRightAreaCorner.x, col);
            this.Walls.Add(new Wall(Orientation.Vertical, wallPosition));
        }

        DefineBounds();
    }

    private void DefineBounds() {
        Vector2Int topRight = this.TopRightAreaCorner;
        Vector2Int botLeft = this.BottomLeftAreaCorner;

        int width = topRight.x - botLeft.x;
        int height = botLeft.y - topRight.y;
        _spawnBounds = new Rect(this.TopLeftAreaCorner.x, this.TopLeftAreaCorner.y, width, height);
    }

    public Rect SpawnBounds { get => this._spawnBounds; }
}