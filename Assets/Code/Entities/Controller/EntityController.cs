using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [SerializeField] private MotionHandler _motionHandler;

    [SerializeField] protected CharacterController _controller;
    [SerializeField] protected Animator _animator;

    public DungeonController _currentDungeon;
    public RoomNode _currentRoom;

    public abstract Vector3 CalculateMoveDirection();

    private Entity _entity;

    protected void Awake() {
        _motionHandler = new MotionHandler(this, 100f);
    }

    protected void Update() {
        Motion.UpdateVelocity();
        if(IsMoving()) {
            bool roomUpdate = UpdateRoom();
        }
    }

    private bool UpdateRoom() {
        RoomNode oldRoom = _currentRoom;
        _currentRoom = _currentDungeon.GetCurrentRoom(this);
        if(!_currentRoom.Equals(oldRoom)) {
            return false;
        }

        if(oldRoom != null) { // check for valid transition
            oldRoom.Entities.Remove(this.Entity);
        }
        if(_currentRoom != null) {
            _currentRoom.Entities.Add(this.Entity);
        }

        return true;
    }

    void Start() {
        _controller.enabled = true;
    }

    public void LookInDirection(float angle) {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void LookAtMovementDirection() { 
        float targetAngle = Mathf.Atan2(Velocity.x, Velocity.z) * Mathf.Rad2Deg;
        float turnAngle = Motion.GetTurnAngle(targetAngle);
        LookInDirection(turnAngle);
    }

    public void EntityMove()
    {
        // Speed should only multiply Velocity x and z. Let Gravity do its thing
        Velocity = new Vector3(Entity.Speed * Velocity.x, Velocity.y, Entity.Speed * Velocity.z);
        Controller.Move(Velocity * Time.deltaTime);
    }

    public bool IsMoving() {
        return (Velocity.x != 0 || Velocity.z != 0);
    }

    public AnimatorStateInfo GetAnimatorStateInfo(int index) { 
        return Animator.GetCurrentAnimatorStateInfo(index); 
    }

    // Getters and Setters
    public Animator Animator { get { return this._animator; } }
    public MotionHandler Motion { get => this._motionHandler; }

   public float Speed { get { return Entity.Speed; } }

    public Vector3 Velocity { 
        get { return Motion.Velocity; }
        set { Motion.Velocity = value; } 
    }
    public Vector3 RelativeVelocity { 
        get { return Motion.RelativeVelocity; }
        set { Motion.RelativeVelocity = value; } 
    }

    public CharacterController Controller { get { return this._controller; } }


    public Entity Entity { get => this._entity; set => this._entity = value; }
    
    public DungeonController CurrentDungeon { get => this._currentDungeon; set => this._currentDungeon = value; }
    public RoomNode CurrentRoom { get => this._currentRoom; set => this._currentRoom = value; }

}