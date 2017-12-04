using UnityEngine;

public class FanSpottedState : AFanState
{
	private float m_SqueakTimer = 0f;
	private float m_SqueakDelay = 0f;

	public FanSpottedState(GirlAI fan) : base(EFanStateID.Spotted, fan)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Fan.UnlockNavigation();
		m_Fan.HeartParticles.Play();
        GirlsManager.Instance.SetSpotted(m_Fan);
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();
		m_Fan.SetAgentSpeed(m_Fan.Params.RunSpeed);

		PlayerController player = GameManager.Instance.Player;
		float dist = Vector3.Distance(player.CachedTransform.position, m_Fan.CachedTransform.position);
		if (dist <= m_Fan.Params.SpottedAttackDistance)
		{
			player.ReceiveAttack();
		}

        if ( !m_Fan.m_AudioSource.isPlaying )
        {
			m_SqueakTimer += GameManager.Instance.DeltaTime;
			if (m_SqueakTimer > m_SqueakDelay)
			{
				m_SqueakTimer = 0f;
				m_SqueakDelay = Random.Range(m_Fan.Params.SqueakDelay.x, m_Fan.Params.SqueakDelay.y);

				int clipID = Random.Range(0, m_Fan.m_Squeaking.Length);
				m_Fan.m_AudioSource.PlayOneShot(m_Fan.m_Squeaking[clipID] );
			}
        }
	}

	protected override void HandleFixedUpdate()
	{
		base.HandleFixedUpdate();

		PlayerController player = GameManager.Instance.Player;
		m_Fan.SetTargetDestination(player.Locomotion.GetTargetPositionForCurrentVelocity());
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.HeartParticles.Stop();
		m_Fan.LockNavigation();
	}
}
