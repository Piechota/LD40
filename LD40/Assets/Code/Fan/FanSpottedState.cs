using UnityEngine;

public class FanSpottedState : AFanState
{
	private const float SPOTTED_SPEED = 7f;

	public FanSpottedState(GirlAI fan) : base(EFanStateID.Spotted, fan)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Fan.UnlockNavigation();
		m_Fan.SetAgentSpeed(SPOTTED_SPEED);
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();

		PlayerController player = GameManager.Instance.Player;
		Vector3 playerPosition = player.CachedTransform.position;
		m_Fan.SetTargetDestination(playerPosition);
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.LockNavigation();
	}
}
