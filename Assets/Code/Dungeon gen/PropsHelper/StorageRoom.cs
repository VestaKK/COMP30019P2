using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    public static void PopulateStorageRoom(RoomNode room, GameObject[] shelvesObjectsList)
    {
        CorridorNode corridor = room.ConnectedNodes[0].Item1;
        RelativePosition entranceRelativeToRoom = 
            StructureHelper.GiveRelativePosition(room.Center, corridor.Center);
        
        int roulette = Random.Range(0, 2);

        // Rows of shelves
        if (roulette == 0)
        {
            float row1 = room.BottomLeftAreaCorner.y + room.Length * (3f/8f);
            float row2 = room.TopRightAreaCorner.y - room.Length * (3f/8f);

            int shelvesPerRow = room.Width / 4; // shelves take width of 2, and we only occupy half of the room width  
            float rowWidth = shelvesPerRow * 2 + 0.1f;
            float fromLeft = room.BottomLeftAreaCorner.x + ((room.Width - rowWidth) / 2.0f) + 1;
            for (int i = 0; i < shelvesPerRow; i++)
            {
                GameObject shelf = 
                    shelvesObjectsList[Random.Range(0, shelvesObjectsList.Length - 1)];
                room.Props.Add(
                    new Prop(
                        shelf,
                        new Vector3(fromLeft + i*2, 0, row1)));
                shelf = shelvesObjectsList[Random.Range(0, shelvesObjectsList.Length - 1)];
                room.Props.Add(
                    new Prop(
                        shelf,
                        new Vector3(fromLeft + i*2, 0, row2)));
            }
        } 
        else
        {
            float row1 = room.BottomLeftAreaCorner.x + room.Width * (3f/8f);
            float row2 = room.TopRightAreaCorner.x - room.Width * (3f/8f);

            int shelvesPerRow = room.Length / 4; // shelves take width of 2, and we only occupy half of the room width  
            float rowWidth = shelvesPerRow * 2 + 0.1f;
            float fromBottom = room.BottomLeftAreaCorner.y + ((room.Length - rowWidth) / 2.0f) + 1;
            for (int i = 0; i < shelvesPerRow; i++)
            {
                GameObject shelf = 
                    shelvesObjectsList[Random.Range(0, shelvesObjectsList.Length - 1)];
                room.Props.Add(
                    new Prop(
                        shelf,
                        new Vector3(row1, 0, fromBottom + i*2),
                        Quaternion.Euler(0f, 0f, 90f)));
                shelf = shelvesObjectsList[Random.Range(0, shelvesObjectsList.Length - 1)];
                room.Props.Add(
                    new Prop(
                        shelf,
                        new Vector3(row2, 0, fromBottom + i*2),
                        Quaternion.Euler(0f, 0f, 90f)));
            }
        }
    }
}