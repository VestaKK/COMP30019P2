using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    public static void PopulateComputerRoom(
        RoomNode room, 
        GameObject computerObject, 
        GameObject tileObject, 
        GameObject pillarObject, 
        GameObject[] cornerPropsList)
    {
        Vector2 center = room.Center;
        float widthSpace = room.Width / 4f;
        float lengthSpace = room.Length / 4f;
        float computerPillarSeparationX = 
            GetObjectBounds(computerObject).x / 2f +
            GetObjectBounds(pillarObject).x / 2f;
        float computerPillarSeparationZ = 
            GetObjectBounds(computerObject).z / 2f +
            GetObjectBounds(pillarObject).z / 2f;

        room.Props.Add(new Prop(
            computerObject,
            new Vector3(center.x, 0, center.y + lengthSpace),
            Quaternion.Euler(0f,0f,180f)
        ));
        room.Props.Add(new Prop(
            tileObject,
            new Vector3(center.x, 0, center.y + lengthSpace)
        ));
        room.Props.Add(new Prop(
            pillarObject,
            new Vector3(center.x, 0, center.y + lengthSpace + computerPillarSeparationZ)
        ));

        room.Props.Add(new Prop(
            computerObject,
            new Vector3(center.x, 0, center.y - lengthSpace),
            Quaternion.Euler(0f,0f,0f)
        ));
        room.Props.Add(new Prop(
            tileObject,
            new Vector3(center.x, 0, center.y - lengthSpace)
        ));
        room.Props.Add(new Prop(
            pillarObject,
            new Vector3(center.x, 0, center.y - lengthSpace - computerPillarSeparationZ)
        ));

        room.Props.Add(new Prop(
            computerObject,
            new Vector3(center.x + widthSpace, 0, center.y),
            Quaternion.Euler(0f,0f,-90f)
        ));
        room.Props.Add(new Prop(
            tileObject,
            new Vector3(center.x + widthSpace, 0, center.y)
        ));
        room.Props.Add(new Prop(
            pillarObject,
            new Vector3(center.x + widthSpace + computerPillarSeparationX, 0, center.y)
        ));

        room.Props.Add(new Prop(
            computerObject,
            new Vector3(center.x - widthSpace, 0, center.y),
            Quaternion.Euler(0f,0f,90f)
        ));
        room.Props.Add(new Prop(
            tileObject,
            new Vector3(center.x - widthSpace, 0, center.y)
        ));
        room.Props.Add(new Prop(
            pillarObject,
            new Vector3(center.x - widthSpace - computerPillarSeparationX, 0, center.y)
        ));

        if (Random.Range(0,2) == 0) PopulateRoomCorners(room, cornerPropsList);
    }
}