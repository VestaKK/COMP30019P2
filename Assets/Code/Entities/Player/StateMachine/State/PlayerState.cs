public abstract class PlayerState 
{
    protected PlayerContext _ctx;
    protected StateManager _stateManager;

    protected PlayerState(PlayerContext playerContext, StateManager stateManager) 
    {
        _ctx = playerContext;
        _stateManager = stateManager;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
    public abstract void CheckSwitchStates();
}
