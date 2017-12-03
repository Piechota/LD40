using UnityEngine;

public class FanIdleState : AFanState
{
	public AEvent OnPlayerSpotted = new AEvent();

	private float m_RotationTimer = 0f;
	private Vector3 m_TargetForward;
	private readonly static Vector2 m_RotationDelay = new Vector2(3f, 5f);
	private const float m_RotationSpeed = 5f;
	private const float m_SpotSpeed = 3f;

	public FanIdleState(GirlAI partner) : base(EFanStateID.Idle, partner)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();

		UpdateTargetRotation();
		DetectPlayer();
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
	}

	private void DetectPlayer()
	{
		float spotValue = GameManager.Instance.DeltaTime * m_SpotSpeed;
		if (m_Fan.DetectPlayer())
		{
			m_Fan.UpdateSpotValue(spotValue);
			if (m_Fan.SpotValue >= 1)
			{
				m_Fan.ResetSpotValue();
				OnPlayerSpotted.Invoke();
			}
		}
		else
		{
			m_Fan.UpdateSpotValue(-spotValue);
		}
	}
}
