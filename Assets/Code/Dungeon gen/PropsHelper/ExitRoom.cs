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
        room.Props.Add(
            new Prop(
                torchObject, 
                new Vector3(
                    room.BottomLeftAreaCorner.x + xOffset, 
                    0, 
                    room.BottomLeftAreaCorner.y + zOffset)));
        room.Props.Add(
            new Prop(
                torchObject, 
                new Vector3(
                    room.BottomRightAreaCorner.x - xOffset, 
                    0, 
                    room.BottomRightAreaCorner.y + zOffset)));
        room.Props.Add(
            new Prop(
                torchObject, 
                new Vector3(
                    room.TopLeftAreaCorner.x + xOffset, 
                    0, 
                    room.TopLeftAreaCorner.y - zOffset)));
        room.Props.Add(
            new Prop(
                torchObject, 
                new Vector3(
                    room.TopRightAreaCorner.x - xOffset, 
                    0, 
                    room.TopRightAreaCorner.y - zOffset)));
    }
}