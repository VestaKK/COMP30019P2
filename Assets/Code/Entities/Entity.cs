using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected EntityController _controller;

    [SerializeField] float _speed;

    public float DistanceToSq(Entity other) {
        float dX = other.Position.x - this.Position.x;
        float dY = other.Position.y - this.Position.y;
        dX *= dX;
        dY *= dY;

        return dX + dY;
    }

    public float DistanceTo(Entity other) {
        return Mathf.Sqrt(DistanceToSq(other));
    }

    // Getters and Setters

    public float Speed { get => this._speed; }
    public EntityController EntityController { get => this._controller; }

    public Transform ObjectTransform { get => this.gameObject.transform; }
    public Vector3 Position { get => ObjectTransform.position; }
}
