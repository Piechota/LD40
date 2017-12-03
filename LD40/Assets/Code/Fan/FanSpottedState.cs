using UnityEngine;

public class FanSpottedState : AFanState
{
	public FanSpottedState(GirlAI fan) : base(EFanStateID.Spotted, fan)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Fan.UnlockNavigation();
	}   

	protected override void HandleUpdate()
	{
		base.HandleUpdate();
		m_Fan.SetAgentSpeed(m_Fan.Params.RunSpeed);

		PlayerController player = GameManager.Instance.Player;
		m_Fan.SetTargetDestination(player.Locomotion.GetTargetPositionForCurrentVelocity());

		float dist = Vector3.Distance(player.CachedTransform.position, m_Fan.CachedTransform.position);
		if (dist <= m_Fan.Params.SpottedAttackDistance)
		{
			player.ReceiveAttack();
		}
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.LockNavigation();
	}
}
