using System.Collections;
using System.Collections.Generic;

public class StateManager
{
    public enum State
    {
        Idle,
        Walk,
        Roll,
        Attack
    }

    private PlayerState _currentState;
    private PlayerController _player;
    private readonly Dictionary<State, PlayerState> _states = new Dictionary<State, PlayerState>();

    public StateManager(PlayerController entity, State initialState) 
    {
        _player = entity;
        _states.Add(State.Idle, new IdleState(this));
        _states.Add(State.Walk, new WalkState(this));
        _states.Add(State.Roll, new RollState(this));
        _states.Add(State.Attack, new AttackState(this));
        SwitchState(GetState(initialState));
    }

    public PlayerState GetState(State s) {
        return _states[s];
    }
    public PlayerState Idle() { 
        return GetState(State.Idle);
    }

    public PlayerState Walk() {
        return  GetState(State.Walk);
    }

    public PlayerState Roll() {
        return GetState(State.Roll);
    }

    public PlayerState Attack() {
        return GetState(State.Attack);
    }

    public void Update() {
        _currentState.Update();
        _currentState.CheckSwitchStates();
    }

    public void SwitchState(PlayerState newState) {
        if (newState == null) return;
        if(_currentState != null) {
            _currentState.Exit();
        }
        _currentState = newState;
        newState.Enter();
    }

    public PlayerController Player { get { return _player; } }
    public PlayerState CurrentState { get { return _currentState; } }
}
