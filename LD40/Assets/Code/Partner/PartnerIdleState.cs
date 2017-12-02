using UnityEngine;

public class PartnerIdleState : APartnerState
{
	public AEvent OnWaitFinished = new AEvent();

	private float m_RotationTimer = 0f;
	private Vector3 m_TargetForward;
	private readonly static Vector2 m_RotationDelay = new Vector2(3f, 5f);
	private const float m_RotationSpeed = 5f;

	public PartnerIdleState(GirlAI partner) : base(EPartnerStateID.Idle, partner)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Partner.PickupTrigger.OnTriggerEntered.AddListener(HandlePickupTriggerEntered);
		m_Partner.PickupTrigger.OnTriggerExited.AddListener(HandlePickupTriggerExited);
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();

		UpdateTargetRotation();

		m_Partner.UpdateWaitTimer();
		if (m_Partner.TimerValue <= 0)
		{
			OnWaitFinished.Invoke();
		}
	}

	private void UpdateTargetRotation()
	{
		m_RotationTimer -= Time.deltaTime;
		if (m_RotationTimer <= 0)
		{
			m_RotationTimer = Random.Range(m_RotationDelay.x, m_RotationDelay.y);

			Vector2 circleForward = Random.insideUnitCircle;
			m_TargetForward = new Vector3(circleForward.x, 0, circleForward.y).normalized;
		}

		Vector3 forward = Vector3.Lerp(m_Partner.CachedTransform.forward, m_TargetForward, Time.deltaTime * m_RotationSpeed);
		m_Partner.SetTargetForward(forward);
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Partner.PickupTrigger.OnTriggerEntered.RemoveListener(HandlePickupTriggerEntered);
		m_Partner.PickupTrigger.OnTriggerExited.RemoveListener(HandlePickupTriggerExited);
	}

    private void HandlePickupTriggerEntered(Collider col)
	{
		if (col.gameObject.layer == PlayerController.LAYER)
		{
			GameManager.Instance.Player.AddPickupOption(m_Partner);
		}
	}

	private void HandlePickupTriggerExited(Collider col)
	{
		if (col.gameObject.layer == PlayerController.LAYER)
		{
			GameManager.Instance.Player.RemovePickupOption(m_Partner);
		}
	}
}
