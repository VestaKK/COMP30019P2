using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySpawner : Spawner<Enemy> {
    [SerializeField] protected Canvas _healthBarCanvas;

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
        return newEnemy;
    }
}