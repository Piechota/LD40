using System.Collections.Generic;

public class FiniteStateMachine
{
	public AEvent OnStateChanged = new AEvent();		// called when first-level children of this FSM change
	public AEvent OnHierarchyChanged = new AEvent();	// called when any FSMs in the hierarchy change current state

	private Dictionary<int, AState> m_States = new Dictionary<int, AState>();

	public int CurrentStateId { get; private set; }

	public FiniteStateMachine()
	{
		CurrentStateId = AState.INVALID_STATE;
	}

	public void Update()
	{
		AState currentState = GetCurrentState();
		if (currentState != null)
		{
			currentState.Update();
		}
	}

	public void AddState(AState state)
	{
		state.SetParent(this);
		m_States[state.Id] = state;

		state.FSM.OnHierarchyChanged.AddListener(HandleHierarchyChanged);
	}

	public void TransitionTo(AState state)
	{
		TransitionTo(state.Id);
	}

	public void TransitionTo(int nextStateId)
	{
		AState prevState = GetCurrentState();
		AState nextState = GetState(nextStateId);
		if (prevState != null)
		{
			prevState.Leave(nextState);
		}

		CurrentStateId = nextStateId;

		if (nextState != null)
		{
			nextState.Enter(prevState);
		}
		else
		{
			CurrentStateId = AState.INVALID_STATE;
		}

		OnStateChanged.Invoke();
		OnHierarchyChanged.Invoke();
	}

	public void Reset()
	{
		TransitionTo(AState.INVALID_STATE);
	}

	public AState GetCurrentState()
	{
		return GetState(CurrentStateId);
	}

	public AState GetState(int stateId)
	{
		if (m_States.ContainsKey(stateId))
		{
			return m_States[stateId];
		}

		return null;
	}

	public Dictionary<int, AState> GetStates()
	{
		return m_States;
	}

	public bool HasStates()
	{
		return m_States.Count > 0;
	}

	public bool IsActive()
	{
		return CurrentStateId != AState.INVALID_STATE;
	}

	private void HandleHierarchyChanged()
	{
		OnHierarchyChanged.Invoke();
	}
}
