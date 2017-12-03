using UnityEngine;
using UnityEngine.AI;

public class FanRoamingState : AFanState
{
	public AEvent OnPlayerSpotted = new AEvent();

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
		m_Fan.DetectPlayer();

		if (m_Fan.GetDistance() <= TARGET_CHANGE_DISTANCE)
		{
            m_Fan.SetIdleState();
        }
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Fan.LockNavigation();
		m_Fan.SetConeActive(false);
	}

	public void RandomizeTargetPoint()
	{
        Transform worldBox = GameManager.Instance.WorldBox.transform;
        Vector3 worldBoxScale = worldBox.localScale;
        Vector3 worldBoxPosition = worldBox.position;
        NavMeshHit hitInfo;
        Vector3 offset = Vector3.zero;
        offset.x = worldBoxScale.x * (0.5f * (Random.value * 2f - 1f));
        offset.z = worldBoxScale.z * (0.5f * (Random.value * 2f - 1f));

        Vector3 targetPoint = worldBoxPosition + offset;
        if (NavMesh.SamplePosition(targetPoint, out hitInfo, 2f, NavMesh.AllAreas))
        {
		    m_Fan.SetTargetDestination( hitInfo.position );
        }
    }
}
