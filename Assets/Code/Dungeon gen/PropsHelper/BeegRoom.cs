using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    public static void PopulateBeegRoom(
        RoomNode room, 
        GameObject pillarObject,
        GameObject statueObject, 
        GameObject[] cornerPropsList)
    {
        Vector2Int bottomLeft = room.BottomLeftAreaCorner;
        Vector2Int bottomRight = room.BottomRightAreaCorner;
        Vector2Int topLeft = room.TopLeftAreaCorner;
        Vector2Int topRight = room.TopRightAreaCorner;

        Vector2 center = StructureHelper.CalculateMiddlePoint(bottomLeft, topRight);
        Vector2 bottomLeftPillar = Vector2.Lerp(center, bottomLeft, 0.5f);
        Vector2 bottomRightPillar = Vector2.Lerp(center, bottomRight, 0.5f);
        Vector2 topLeftPillar = Vector2.Lerp(center, topLeft, 0.5f);
        Vector2 topRightPillar = Vector2.Lerp(center, topRight, 0.5f);

        room.Props.Add(
            new Prop(
                pillarObject, 
                new Vector3(bottomLeftPillar.x, 0, bottomLeftPillar.y)));
        room.Props.Add(
            new Prop(
                pillarObject, 
                new Vector3(bottomRightPillar.x, 0, bottomRightPillar.y)));
        room.Props.Add(
            new Prop(
                pillarObject, 
                new Vector3(topLeftPillar.x, 0, topLeftPillar.y)));
        room.Props.Add(
            new Prop(
                pillarObject, 
                new Vector3(topRightPillar.x, 0, topRightPillar.y)));

        room.Props.Add(new Prop(statueObject, new Vector3(room.Center.x, 0, room.Center.y)));

        if (Random.Range(0,2) == 0) PopulateRoomCorners(room, cornerPropsList);
    }
}