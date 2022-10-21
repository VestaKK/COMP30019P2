using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Spawner<EntityType> : MonoBehaviour
    where EntityType : Entity {
        [SerializeField] private List<EntityType> _prefabs;

        [SerializeField] private int minSpawnsPerRoom;
        [SerializeField] private int maxSpawnsPerRoom;
        [SerializeField] protected Camera _camera;

        void Awake() {
            _camera = Camera.main;
        }

        public abstract EntityType SpawnEntity(Transform parentTransform, Vector3 offset, Quaternion rotation);
        public abstract EntityType SpawnEntity(Transform parentTransform, Vector3 offset, Quaternion rotation, EntityType prefab);
        public abstract EntityType SpawnEntity(); 

        public int GetSpawnCount() {
            return Random.Range(minSpawnsPerRoom, maxSpawnsPerRoom);
        }
        public List<EntityType> Prefabs { get => _prefabs; }

        public CharacterController GetController(EntityType prefab) {
            return prefab.EntityController.Controller;
        }

}