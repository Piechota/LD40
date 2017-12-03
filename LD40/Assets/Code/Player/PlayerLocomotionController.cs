using UnityEngine;
using UnityEngine.AI;

public class PlayerLocomotionController : APlayerComponent
{
	[Header("Movement")]
	[SerializeField]
	private float m_RunSpeed = 10f;
	[SerializeField]
	private float m_Acceleration = 10f;
	[SerializeField]
	private float m_Deceleration = 15f;

    private Vector3 m_CurrentVelocity;
	private Vector3 m_TargetDirection;
	private Vector3 m_TargetPosition;

	private Vector3 m_PlayerScale;

	private void Awake()
	{
	}

	protected override void HandleInitialization(PlayerController player)
	{
	}

	protected override void HandleUninitialization()
	{
	}

	public override void UpdateBehaviour()
	{
		base.UpdateBehaviour();

        m_TargetDirection = m_Player.Input.InputVector;

		UpdateVelocity();
		UpdateNavmesh();
		UpdatePosition();

		m_TargetDirection = Vector3.zero;    // for next turn
	}

	public void ResetVelocity()
	{
		m_CurrentVelocity = Vector3.zero;
		m_TargetDirection = Vector3.zero;
	}

	public void ForceVelocity(Vector3 velocity)
	{
		m_CurrentVelocity = velocity;
	}

	public void AddVelocity(Vector3 velocity)
	{
		m_CurrentVelocity += velocity;
	}

	private void UpdateVelocity()
	{
		float movementMod = m_RunSpeed;
		float targetSpeed = m_TargetDirection.magnitude * movementMod;
		Vector3 targetVelocity = m_TargetDirection * targetSpeed;

		// check if we're accelerating or decelerating
		bool isAccelerating = (targetSpeed >= m_CurrentVelocity.magnitude);
		float acceleration = isAccelerating ? m_Acceleration : m_Deceleration;

		// modify velocity
		Vector3 velocityDiff = targetVelocity - m_CurrentVelocity;
		m_CurrentVelocity += velocityDiff * acceleration * GameManager.Instance.DeltaTime;
	}

	private void UpdateNavmesh()
	{
		Vector3 offset = m_CurrentVelocity * GameManager.Instance.DeltaTime;
		m_TargetPosition = CachedTransform.localPosition + offset;

		NavMeshHit hit;
		if (NavMesh.SamplePosition(m_TargetPosition, out hit, 1f, 1))
		{
			m_TargetPosition = hit.position;
		}
	}

	private void UpdatePosition()
	{
		CachedTransform.localPosition = m_TargetPosition;
	}
}