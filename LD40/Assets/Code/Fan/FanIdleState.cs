using UnityEngine;

public class FanIdleState : AFanState
{
	private float m_RotationTimer = 0f;
	private Vector3 m_TargetForward;

	public FanIdleState(GirlAI partner) : base(EFanStateID.Idle, partner)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Fan.SetConeActive(true);
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();

		UpdateTargetRotation();
		m_Fan.DetectPlayer();
	}

	private void UpdateTargetRotation()
	{
		m_RotationTimer -= GameManager.Instance.DeltaTime;
		if (m_RotationTimer <= 0)
		{
			m_RotationTimer = Random.Range(m_Fan.Params.IdleRotationDelay.x, m_Fan.Params.IdleRotationDelay.y);

			Vector2 circleForward = Random.insideUnitCircle;
			m_TargetForward = new Vector3(circleForward.x, 0, circleForward.y).normalized;
		}

		float lerp = GameManager.Instance.DeltaTime * m_Fan.Params.IdleRotationSpeed;
		Vector3 forward = Vector3.Lerp(m_Fan.CachedTransform.forward, m_TargetForward, lerp);
		m_Fan.SetTargetForward(forward);
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.SetConeActive(false);
	}
}
