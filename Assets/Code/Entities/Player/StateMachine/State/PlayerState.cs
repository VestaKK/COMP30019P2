public abstract class PlayerState 
{
    protected StateManager _stateManager;

    protected PlayerState(StateManager stateManager) 
    {
        _stateManager = stateManager;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
    public abstract void CheckSwitchStates();

    public PlayerController Player { get { return _stateManager.Player; } }
}
