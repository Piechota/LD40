using UnityEngine;

public class PartnerIdleState : APartnerState
{
	public AEvent OnWaitFinished = new AEvent();

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

		m_Partner.UpdateWaitTimer();
		if (m_Partner.TimerValue <= 0)
		{
			OnWaitFinished.Invoke();
		}
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
