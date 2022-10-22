using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySpawner : Spawner<Enemy> {
    [SerializeField] protected Canvas _healthBarCanvas;
    [SerializeField] private float _itemDropChance = 0.125f;
    [SerializeField] private GameObject[] _items;

    public override Enemy SpawnEntity()
    {
        int rand = Random.Range(0, Prefabs.Count - 1);
        Enemy prefab = Prefabs[rand];
        return SpawnEntity(transform, Vector3.zero, Quaternion.identity, prefab);
    }

    public override Enemy SpawnEntity(Transform parentTransform, Vector3 offset, Quaternion rotation)
    {
        int rand = Random.Range(0, Prefabs.Count - 1);
        Enemy prefab = Prefabs[rand];
        Enemy newEnemy = Instantiate(
            prefab,
            parentTransform.position + offset,
            Quaternion.identity,
            parentTransform);
        newEnemy.SetupHealthbar(_healthBarCanvas, _camera);
        if (Random.Range(0f, 1f) < _itemDropChance)
            newEnemy.item = _items[Random.Range(0, _items.Length)];
        return newEnemy;
    }

    public override Enemy SpawnEntity(Transform parentTransform, Vector3 offset, Quaternion rotation, Enemy prefab)
    {
        Enemy newEnemy = Instantiate(
            prefab,
            parentTransform.position + offset,
            Quaternion.identity,
            parentTransform);
        newEnemy.SetupHealthbar(_healthBarCanvas, _camera);
        if (Random.Range(0f, 1f) < _itemDropChance)
            newEnemy.item = _items[Random.Range(0, _items.Length)];
        return newEnemy;
    }
}