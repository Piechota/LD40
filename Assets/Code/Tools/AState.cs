public abstract class AState
{
	public AEvent OnStateEntered = new AEvent();
	public AEvent OnStateLeft = new AEvent();

	public FiniteStateMachine ParentFSM = new FiniteStateMachine();
	public FiniteStateMachine FSM = new FiniteStateMachine();

	public int Id { get; private set; }

	public const int INVALID_STATE = -1;

	public AState(int id)
	{
		Id = id;
	}

	public void SetParent(FiniteStateMachine parent)
	{
		ParentFSM = parent;
	}

	public void Enter(AState prevState)
	{
		HandleEnter(prevState);
		OnStateEntered.Invoke();
	}

	protected abstract void HandleEnter(AState prevState);

	public void Update()
	{
		HandleUpdate();

		if (FSM.IsActive())
		{
			FSM.Update();
		}
	}

	protected virtual void HandleUpdate()
	{
	}

	public void Leave(AState nextState)
	{
		HandleLeave(nextState);
		if (FSM.IsActive())
		{
			FSM.Reset();
		}

		OnStateLeft.Invoke();
	}

	protected abstract void HandleLeave(AState nextState);

	public override string ToString()
	{
		return GetType().ToString();
	}
}
