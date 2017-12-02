public class PartnerPairState : APartnerState
{
	public PartnerPairState(GirlAI partner) : base(EPartnerStateID.Pair, partner)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Partner.StartFollowing();
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();
		m_Partner.FollowPlayer();
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Partner.StopFollowing();
	}
}
