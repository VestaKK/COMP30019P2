using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Spawner<EntityType> : MonoBehaviour
    where EntityType : Entity {
        [SerializeField] private List<EntityType> _prefabs;

        [SerializeField] private int minSpawnsPerRoom;
        [SerializeField] private int maxSpawnsPerRoom;
        [SerializeField] protected Camera _camera;

        [SerializeField] public int maxSpawnCheckCount;

        void Awake() {
            _camera = Camera.main;
        }

        public abstract EntityType SpawnEntity(Transform parentTransform, Vector3 offset, Quaternion rotation);
        public abstract EntityType SpawnEntity(Transform parentTransform, Vector3 offset, Quaternion rotation, EntityType prefab);
        public abstract EntityType SpawnEntity(); 

        public int GetSpawnCount() {
            return Random.Range(MinSpawnsPerRoom, MaxSpawnsPerRoom);
        }
        public List<EntityType> Prefabs { get => _prefabs; }

    public CharacterController GetController(EntityType prefab) {
            return prefab.EntityController.Controller;
        }


    public int MaxSpawnsPerRoom { get => maxSpawnsPerRoom; set => maxSpawnsPerRoom = value; }
    public int MinSpawnsPerRoom { get => minSpawnsPerRoom; set => minSpawnsPerRoom = value; }
}