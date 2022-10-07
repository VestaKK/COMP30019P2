using System.Collections;
using System.Collections.Generic;

public class StateManager
{
    private enum State
    {
        Idle,
        Walk,
        Roll,
        Attack
    }

    private PlayerContext _playerContext;
    private readonly Dictionary<State, PlayerState> _states = new Dictionary<State, PlayerState>();

    public StateManager(PlayerContext playerContext) 
    {
        _playerContext = playerContext;
        _states.Add(State.Idle, new IdleState(playerContext, this));
        _states.Add(State.Walk, new WalkState(playerContext, this));
        _states.Add(State.Roll, new RollState(playerContext, this));
        _states.Add(State.Attack, new AttackState(playerContext, this));
    }

    public PlayerState Idle() { 
        return _states[State.Idle];
    }

    public PlayerState Walk() {
        return _states[State.Walk];
    }

    public PlayerState Roll() {
        return _states[State.Roll];
    }

    public PlayerState Attack() {
        return _states[State.Attack];
    }

    public void SwitchState(PlayerState newState) {
        if (newState == null) return;
        _playerContext.CurrentState.Exit();
        _playerContext.CurrentState = newState;
        newState.Enter();
    }
}
