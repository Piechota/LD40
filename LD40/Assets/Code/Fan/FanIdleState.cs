using UnityEngine;

public class FanIdleState : AFanState
{
	private float m_RotationTimer = 0f;
	private Vector3 m_TargetForward;
	private readonly static Vector2 m_RotationDelay = new Vector2(3f, 5f);
	private const float m_RotationSpeed = 5f;
    private readonly static Vector2 m_IdleRandom = new Vector2(6f,10f);
    private float m_IdleTime;

    public FanIdleState(GirlAI partner) : base(EFanStateID.Idle, partner)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Fan.SetConeActive(true);
        m_IdleTime = Random.Range(m_IdleRandom.x, m_IdleRandom.y);
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();

		m_Fan.DetectPlayer();
		UpdateTargetRotation();

        m_IdleTime -= GameManager.Instance.DeltaTime;
        if ( m_IdleTime < 0f )
        {
            m_Fan.SetRoamingState();
        }

    }

	private void UpdateTargetRotation()
	{
		m_RotationTimer -= GameManager.Instance.DeltaTime;
		if (m_RotationTimer <= 0)
		{
			m_RotationTimer = Random.Range(m_RotationDelay.x, m_RotationDelay.y);

			Vector2 circleForward = Random.insideUnitCircle;
			m_TargetForward = new Vector3(circleForward.x, 0, circleForward.y).normalized;
		}

		Vector3 forward = Vector3.Lerp(m_Fan.CachedTransform.forward, m_TargetForward, GameManager.Instance.DeltaTime * m_RotationSpeed);
		m_Fan.SetTargetForward(forward);
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.SetConeActive(false);
	}
}
