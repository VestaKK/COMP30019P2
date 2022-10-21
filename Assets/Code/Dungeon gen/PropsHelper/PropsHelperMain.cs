using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    private static void PopulateRoomCorners(RoomNode room, GameObject[] propsList)
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
    public static Vector3 GetObjectBounds(GameObject obj)
    {
        return obj.GetComponent<Renderer>().bounds.size;
    }

    // Check if a coordinate in a room is free from any doorways such that
    // a prop there will not obstruct it
    private static bool CheckClearFromDoor(RoomNode room, Vector2 coord, GameObject prop)
    {
        Vector3 propBounds = GetObjectBounds(prop);
        float rangeSq = 
            (float) (Math.Pow(propBounds.x / 2, 2) + Math.Pow(propBounds.z / 2, 2)) + 1f;

        return room.Doors.All(door => 
            door.orientation == Orientation.Horizontal 
                ? (coord - door.coordinates - new Vector2(0.5f, 0f)).sqrMagnitude > rangeSq 
                : (coord - door.coordinates - new Vector2(0f, 0.5f)).sqrMagnitude > rangeSq);
    }
}