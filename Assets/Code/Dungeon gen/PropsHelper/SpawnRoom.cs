using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random; 

public static partial class PropsHelper
{
    public static void PopulateSpawnRoom(
        RoomNode room, 
        GameObject spawnFloorTile, 
        GameObject spawnFloorTileEdge, 
        GameObject spawnFloorTileCorner)
    {
        room.Props.Add(
            new Prop(spawnFloorTile, new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y)));

        float floorTileLength = GetObjectBounds(spawnFloorTile).x;

        room.Props.Add(
            new Prop(
                spawnFloorTileEdge, 
                new Vector3(room.SpawnPoint.x - floorTileLength, 0, room.SpawnPoint.y), 
                Quaternion.Euler(0f,0f,180f)));
        room.Props.Add(
            new Prop(
                spawnFloorTileEdge, 
                new Vector3(room.SpawnPoint.x + floorTileLength, 0, room.SpawnPoint.y)));
        room.Props.Add(
            new Prop(
                spawnFloorTileEdge, 
                new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y - floorTileLength), 
                Quaternion.Euler(0f,0f,90f)));
        room.Props.Add(
            new Prop(
                spawnFloorTileEdge, 
                new Vector3(room.SpawnPoint.x, 0, room.SpawnPoint.y + floorTileLength), 
                Quaternion.Euler(0f,0f,-90f)));

        room.Props.Add(
            new Prop(
                spawnFloorTileCorner, 
                new Vector3(
                    room.SpawnPoint.x - floorTileLength, 
                    0, 
                    room.SpawnPoint.y - floorTileLength), 
                Quaternion.Euler(0f,0f,90f)));
        room.Props.Add(
            new Prop(spawnFloorTileCorner, 
            new Vector3(
                room.SpawnPoint.x - floorTileLength, 
                0, 
                room.SpawnPoint.y + floorTileLength), 
            Quaternion.Euler(0f,0f,180f)));
        room.Props.Add(
            new Prop(
                spawnFloorTileCorner, 
                new Vector3(
                    room.SpawnPoint.x + floorTileLength, 
                    0, 
                    room.SpawnPoint.y - floorTileLength)));
        room.Props.Add(
            new Prop(
                spawnFloorTileCorner, 
                new Vector3(
                    room.SpawnPoint.x + floorTileLength, 
                    0, 
                    room.SpawnPoint.y + floorTileLength), 
                Quaternion.Euler(0f,0f,-90f)));
    }
}