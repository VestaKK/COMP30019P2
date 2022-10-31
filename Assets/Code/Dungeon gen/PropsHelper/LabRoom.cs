using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    public static void PopulateLabRoom(
        RoomNode room, 
        GameObject[] labItemObjectList, 
        GameObject computerObject, 
        GameObject tileObject, 
        GameObject[] cornerPropsList)
    {
        Vector2 center = room.Center;

        GameObject labItem = labItemObjectList[Random.Range(0, labItemObjectList.Length)];
        RelativePosition computerPosition = (RelativePosition) Random.Range(0, 4);
        float computerLabItemSpace = 
            GetObjectBounds(computerObject).z / 2 +
            GetObjectBounds(labItem).z / 2 + 0.5f;

        if ((int) computerPosition >= 2) // left/right
        {
            room.Props.Add(new Prop(
                labItem, 
                new Vector3(center.x, 0, center.y),
                Quaternion.Euler(0f, 0f, 90f)));

            room.Props.Add(new Prop(
                computerObject,
                new Vector3(
                    center.x + (
                        computerPosition == RelativePosition.Left 
                        ? -computerLabItemSpace 
                        : computerLabItemSpace), 
                    0, 
                    center.y),
                computerPosition == RelativePosition.Left 
                ? Quaternion.Euler(0f, 0f, -90f) 
                : Quaternion.Euler(0f, 0f, 90f)
            ));
            
            room.Props.Add(new Prop(
                tileObject,
                new Vector3(
                    center.x + (
                        computerPosition == RelativePosition.Left 
                        ? -computerLabItemSpace 
                        : computerLabItemSpace), 
                    0, 
                    center.y)));
        }
        else // up/down
        {
            room.Props.Add(new Prop(
                labItem, 
                new Vector3(center.x, 0, center.y)));

            room.Props.Add(new Prop(
                computerObject,
                new Vector3(
                    center.x, 
                    0, 
                    center.y + (
                        computerPosition == RelativePosition.Down 
                        ? -computerLabItemSpace 
                        : computerLabItemSpace)),
                computerPosition == RelativePosition.Down 
                ? Quaternion.Euler(0f, 0f, 180f) 
                : Quaternion.Euler(0f, 0f, 0f)
            ));

            room.Props.Add(new Prop(
                tileObject,
                new Vector3(
                    center.x, 
                    0, 
                    center.y + (
                        computerPosition == RelativePosition.Down 
                        ? -computerLabItemSpace 
                        : computerLabItemSpace))));
        }

        if (Random.Range(0,2) == 0) PopulateRoomCorners(room, cornerPropsList);
    }
}