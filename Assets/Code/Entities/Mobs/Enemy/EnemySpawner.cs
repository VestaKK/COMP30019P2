using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySpawner : Spawner<Enemy> {
    [SerializeField] protected Canvas _healthBarCanvas;

    public override Enemy SpawnEntity()
    {
        return SpawnEntity(transform, Vector3.zero, Quaternion.identity);
    }
    public override Enemy SpawnEntity(Transform parentTransform, Vector3 offset, Quaternion rotation)
    {
        Enemy newEnemy = Instantiate(
            Prefab,
            parentTransform.position + offset,
            Quaternion.identity,
            parentTransform);
        newEnemy.SetupHealthbar(_healthBarCanvas, _camera);
        return newEnemy;
    }
}