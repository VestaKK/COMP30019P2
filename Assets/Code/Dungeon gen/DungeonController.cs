using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    [SerializeField] public List<RoomNode> rooms;
    [SerializeField] public List<CorridorNode> corridors;
    [SerializeField] public RoomNode spawnRoom; // Spawn point room
    [SerializeField] public RoomNode exitRoom; // Exit point room

    public RoomNode GetCurrentRoom(EntityController entity) {
            return rooms.Find((room) => room.EntityInBounds(entity.Entity));
    }
}
