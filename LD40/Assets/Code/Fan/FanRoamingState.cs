using UnityEngine;
using UnityEngine.AI;

public class FanRoamingState : AFanState
{
	public AEvent OnPlayerSpotted = new AEvent();

	private Vector3 m_TargetPoint;
	private const float TARGET_DISTANCE = 10f;
	private const float TARGET_CHANGE_DISTANCE = 1f;
	private const float ROAMING_SPEED = 4f;

	public FanRoamingState(GirlAI fan) : base(EFanStateID.Roaming, fan)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Fan.SetConeActive(true);
		m_Fan.UnlockNavigation();
		m_Fan.SetAgentSpeed(ROAMING_SPEED);
		RandomizeTargetPoint();
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();

		m_Fan.SetTargetDestination(m_TargetPoint);
		float dist = Vector3.Distance(m_Fan.CachedTransform.position, m_TargetPoint);
		if (dist <= TARGET_CHANGE_DISTANCE)
		{
			RandomizeTargetPoint();
		}

		m_Fan.DetectPlayer();
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.LockNavigation();
		m_Fan.SetConeActive(false);
	}

	public void RandomizeTargetPoint()
	{
		Vector3 pos = m_Fan.CachedTransform.position + Random.insideUnitSphere * TARGET_DISTANCE;
		NavMeshHit navHit;
		NavMesh.SamplePosition(pos, out navHit, TARGET_DISTANCE, NavMesh.AllAreas);
		m_TargetPoint = navHit.position;
	}
}
