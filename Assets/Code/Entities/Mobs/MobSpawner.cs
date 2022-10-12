using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MobSpawner : Spawner<Mob> {
    [SerializeField] protected Canvas _healthBarCanvas;
    [SerializeField] protected Image _progressImage;

    public override Mob SpawnEntity()
    {
        Mob newMob = Instantiate(
            Prefab,
            transform.position,
            Quaternion.identity,
            transform);
        newMob.SetupHealthbar(_healthBarCanvas, _camera);
        return newMob;
    }
}