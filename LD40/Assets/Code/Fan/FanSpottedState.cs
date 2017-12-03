public class FanSpottedState : AFanState
{
	public FanSpottedState(GirlAI fan) : base(EFanStateID.Spotted, fan)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Fan.StartFollowing();
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();
		m_Fan.FollowPlayer();
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.StopFollowing();
	}
}
