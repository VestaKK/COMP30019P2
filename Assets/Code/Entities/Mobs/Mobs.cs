using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mob : Entity
{
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;

    public abstract void TakeDamage(AttackInfo info);
    public abstract void OnDeath();

    public void SetupHealthbar(Canvas canvas, Camera camera) {
        HealthBar.transform.SetParent(canvas.transform);
        // if(HealthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera)) {
        //     faceCamera.Camera = camera;
        // }
    }

    public ProgressBar HealthBar { get => this.Controller.HealthBar; }


    public void TakeDamage(int dmg) {
        TakeDamage(new AttackInfo(dmg, Vector3.zero,0,Vector3.zero));
    }
}