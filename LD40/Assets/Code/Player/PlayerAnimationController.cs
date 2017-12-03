using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : APlayerComponent
{
	[SerializeField]
	private Transform m_ModelTransform;

	[SerializeField]
	private GameObject m_HiddenBody;
	[SerializeField]
	private GameObject m_ExposedBody;

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

	public void SetExposed(bool set)
	{
		if (!set)
		{
			m_HiddenBody.SetActive(true);
			m_ExposedBody.SetActive(false);
		}
		else
		{
			m_HiddenBody.SetActive(false);
			m_ExposedBody.SetActive(true);
		}
	}
}
