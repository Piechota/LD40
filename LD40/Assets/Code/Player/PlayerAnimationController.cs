using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : APlayerComponent
{
	[SerializeField]
	private Transform m_ModelTransform;

	private Animator m_Animator;

	private void Awake()
	{
		m_Animator = GetComponent<Animator>();
	}

	protected override void HandleInitialization(PlayerController player)
	{
	}

	public override void UpdateBehaviour()
	{
		base.UpdateBehaviour();
		SetOrientation(m_Player.Input.DirectionVector);
	}

	protected override void HandleUninitialization()
	{
	}

	public void SetOrientation(Vector3 direction)
	{
		m_ModelTransform.forward = direction;
	}
}
