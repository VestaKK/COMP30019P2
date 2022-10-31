using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    public static void PopulateExitRoom(RoomNode room, GameObject exitObject, GameObject torchObject)
    {
        room.Props.Add(new Prop(exitObject, new Vector3(room.ExitPoint.x, 0, room.ExitPoint.y)));

        float xOffset = GetObjectBounds(torchObject).x / 2 + 0.1f;
        float zOffset = GetObjectBounds(torchObject).z / 2 + 0.1f;

        Vector2 bottomLeft = new Vector2(
            room.BottomLeftAreaCorner.x + xOffset, 
            room.BottomLeftAreaCorner.y + zOffset);
        Vector2 bottomRight = new Vector2(
            room.BottomRightAreaCorner.x - xOffset, 
            room.BottomRightAreaCorner.y + zOffset);
        Vector2 topLeft = new Vector2(
            room.TopLeftAreaCorner.x + xOffset, 
            room.TopLeftAreaCorner.y - zOffset);
        Vector2 topRight = new Vector2(
            room.TopRightAreaCorner.x - xOffset, 
            room.TopRightAreaCorner.y - zOffset);
        
        if (CheckClearFromDoor(room, bottomLeft, torchObject))
            room.Props.Add(
                new Prop(
                    torchObject, 
                    new Vector3(
                        bottomLeft.x, 
                        0, 
                        bottomLeft.y)));
        if (CheckClearFromDoor(room, bottomRight, torchObject))
            room.Props.Add(
                new Prop(
                    torchObject, 
                    new Vector3(
                        bottomRight.x, 
                        0, 
                        bottomRight.y)));
        if (CheckClearFromDoor(room, topLeft, torchObject))
            room.Props.Add(
                new Prop(
                    torchObject, 
                    new Vector3(
                        topLeft.x, 
                        0, 
                        topLeft.y)));
        if (CheckClearFromDoor(room, topRight, torchObject))
            room.Props.Add(
                new Prop(
                    torchObject, 
                    new Vector3(
                        topRight.x, 
                        0, 
                        topRight.y)));
    }
}