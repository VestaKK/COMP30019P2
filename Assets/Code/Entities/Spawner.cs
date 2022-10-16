using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Spawner<EntityType> : MonoBehaviour
    where EntityType : Entity {
        [SerializeField] private EntityType _prefab;

        [SerializeField] private int minSpawnsPerRoom;
        [SerializeField] private int maxSpawnsPerRoom;
        [SerializeField] protected Camera _camera;

        public abstract Enemy SpawnEntity(Transform parentTransform, Vector3 offset, Quaternion rotation);
        public abstract EntityType SpawnEntity(); 

        public int GetSpawnCount() {
            return Random.Range(minSpawnsPerRoom, maxSpawnsPerRoom);
        }
        public EntityType Prefab { get => _prefab; }
}