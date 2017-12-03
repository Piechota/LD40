using UnityEngine;

public class FanSpottedState : AFanState
{
	private const float SPOTTED_SPEED = 11f;
	private const float SPOTTED_ATTACK_DISTANCE = 1f;

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

		float dist = Vector3.Distance(player.CachedTransform.position, m_Fan.CachedTransform.position);
		if (dist <= SPOTTED_ATTACK_DISTANCE)
		{
			player.ReceiveAttack();
		}
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.LockNavigation();
	}
}
