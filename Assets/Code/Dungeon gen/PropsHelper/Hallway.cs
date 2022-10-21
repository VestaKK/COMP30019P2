using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    public static void PopulateHallway(
        RoomNode room, 
        Orientation orientation, 
        GameObject pillarObject, 
        GameObject[] cornerPropsList)
    {
        if (orientation == Orientation.Horizontal)
        {
            float separation = room.Length / 3.0f;
            float row1 = room.BottomLeftAreaCorner.y + separation * 2;
            float row2 = room.BottomLeftAreaCorner.y + separation;

            separation = room.Width / ((float) Math.Ceiling(room.Width / (separation + 1)));
            for (
                float x = room.BottomLeftAreaCorner.x + separation; 
                x <= room.TopRightAreaCorner.x - separation + 0.1f; 
                x += separation)
            {
                room.Props.Add(new Prop(pillarObject, new Vector3(x, 0, row1)));
                room.Props.Add(new Prop(pillarObject, new Vector3(x, 0, row2)));
            }
        }
        else 
        {
            float separation = room.Width / 3.0f;
            float col1 = room.BottomLeftAreaCorner.x + separation * 2;
            float col2 = room.BottomLeftAreaCorner.x + separation;

            separation = room.Length / ((float) Math.Ceiling(room.Length / (separation + 1)));
            for (
                float y = room.BottomLeftAreaCorner.y + separation; 
                y <= room.TopRightAreaCorner.y - separation + 0.1f; 
                y += separation)
            {
                room.Props.Add(new Prop(pillarObject, new Vector3(col1, 0, y)));
                room.Props.Add(new Prop(pillarObject, new Vector3(col2, 0, y)));
            }
        }

        if (Random.Range(0,2) == 0) PopulateRoomCorners(room, cornerPropsList);
    }
}