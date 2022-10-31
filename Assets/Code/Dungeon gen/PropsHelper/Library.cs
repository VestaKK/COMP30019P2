using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    public static void PopulateLibrary(
        RoomNode room, 
        GameObject[] shelvesObjectsList, 
        GameObject computerObject, 
        GameObject tileObject)
    {
        int roulette = Random.Range(0,3);

        float shelfX = shelvesObjectsList.Select(shelf => GetObjectBounds(shelf).x).Max();
        float shelfZ = shelvesObjectsList.Select(shelf => GetObjectBounds(shelf).z).Max();
        float shelfOffset = shelfZ / 2f + 0.1f;

        GameObject computer = computerObject;
        GameObject tile = tileObject;
        float tileOffset = GetObjectBounds(tile).x / 2;
        int spaceBtwnComputers = 1;

        float from, row1, row2, noOfShelves;
        bool alignTopBottom;
        if (roulette == 1 || (roulette == 3 && room.Width > room.Length)) // Top/bottom
        {
            from = room.BottomLeftAreaCorner.x + shelfX / 2;
            if (room.Width % shelfX != 0) from += (room.Width % shelfX) / 2;
            row1 = room.TopRightAreaCorner.y - shelfOffset;
            row2 = room.BottomLeftAreaCorner.y + shelfOffset;
            noOfShelves = room.Width / shelfX - 1;
            alignTopBottom = true;
        }
        else
        {
            from = room.BottomLeftAreaCorner.y + shelfX / 2;
            if (room.Length % shelfX != 0) from += (room.Length % shelfX) / 2;
            row1 = room.TopRightAreaCorner.x - shelfOffset;
            row2 = room.BottomLeftAreaCorner.x + shelfOffset;
            noOfShelves = room.Length / shelfX - 1;
            alignTopBottom = false;
        }

        for (int i = 0; i < noOfShelves; i++)
        {
            Vector2 coord1 = 
                alignTopBottom
                ? new Vector2(from + i * shelfX, row1)
                : new Vector2(row1, from + i * shelfX);
            Vector2 coord2 = 
                alignTopBottom
                ? new Vector2(from + i * shelfX, row2)
                : new Vector2(row2, from + i * shelfX);

            if (
                (spaceBtwnComputers < 2 || i >= noOfShelves - 1 || Random.Range(0,2) >= 1) 
                && spaceBtwnComputers < 4)
            {
                spaceBtwnComputers++;

                GameObject shelf1 = 
                    shelvesObjectsList[Random.Range(0, shelvesObjectsList.Length - 1)];
                GameObject shelf2 = 
                    shelvesObjectsList[Random.Range(0, shelvesObjectsList.Length - 1)];

                Quaternion rotation = 
                    alignTopBottom 
                    ? Quaternion.identity 
                    : Quaternion.Euler(0f,0f,90f);
                if (CheckClearFromDoor(room, coord1, shelf1))
                    room.Props.Add(new Prop(shelf1, new Vector3(coord1.x, 0, coord1.y), rotation));
                if (CheckClearFromDoor(room, coord2, shelf2))
                    room.Props.Add(new Prop(shelf2, new Vector3(coord2.x, 0, coord2.y), rotation));
            }
            else
            {
                spaceBtwnComputers = 0;

                Vector2 tileCoord1 =
                    alignTopBottom
                    ? new Vector2(from + i * shelfX, row1 + shelfOffset - tileOffset)
                    : new Vector2(row1 + shelfOffset - tileOffset, from + i * shelfX);
                Vector2 tileCoord2 = 
                    alignTopBottom
                    ? new Vector2(from + i * shelfX, row2 - shelfOffset + tileOffset)
                    : new Vector2(row2 - shelfOffset + tileOffset, from + i * shelfX);
                
                if (CheckClearFromDoor(room, tileCoord1, tile))
                {
                    Quaternion rotation = 
                        alignTopBottom
                        ? Quaternion.Euler(0f,0f,180f)
                        : Quaternion.Euler(0f,0f,-90f);
                    room.Props.Add(new Prop(computer, new Vector3(coord1.x, 0, coord1.y), rotation));
                    room.Props.Add(new Prop(tile, new Vector3(tileCoord1.x, 0, tileCoord1.y)));
                }
                if (CheckClearFromDoor(room, tileCoord2, tile))
                {
                    Quaternion rotation = 
                        alignTopBottom
                        ? Quaternion.identity
                        : Quaternion.Euler(0f,0f,90f);
                    room.Props.Add(new Prop(computer, new Vector3(coord2.x, 0, coord2.y), rotation));
                    room.Props.Add(new Prop(tile, new Vector3(tileCoord2.x, 0, tileCoord2.y)));
                }
            }
        }
    }
}