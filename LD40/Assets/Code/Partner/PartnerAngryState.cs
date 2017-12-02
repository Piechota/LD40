using UnityEngine;

public class PartnerAngryState : APartnerState
{
	public PartnerAngryState(GirlAI partner) : base(EPartnerStateID.Angry, partner)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Partner.PickupTrigger.OnTriggerEntered.AddListener(HandlePickupTriggerEntered);
		m_Partner.PickupTrigger.OnTriggerExited.AddListener(HandlePickupTriggerExited);
		m_Partner.StartFollowing();
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();
		m_Partner.FollowPlayer();
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Partner.PickupTrigger.OnTriggerEntered.RemoveListener(HandlePickupTriggerEntered);
		m_Partner.PickupTrigger.OnTriggerExited.RemoveListener(HandlePickupTriggerExited);
		m_Partner.StopFollowing();
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
