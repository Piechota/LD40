using UnityEngine;

public class GameConcertState : AGameState
{
	public readonly AEvent OnConcertFinished = new AEvent();

	private float m_Timer = 0f;
	private Location m_ConcertLocation;
    private float m_PlayerSpeed = 0f;
	private const float CONCERT_DURATION = 4f;

    private Vector3[] m_PlayerPoints = new Vector3[3];
    private int m_PlayerPointID = 0;

    private float m_AudioVolume = 0f;

	public GameConcertState() : base(EGameState.Concert)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Mgr.SetGameTimerPause(true);
		m_Mgr.Player.SetInputLock(true);

		m_Timer = 0f;

		if (m_ConcertLocation.Type == ELocationType.Back)
		{
			POIManager.Instance.SetCutsceneCameraBackActive(true);
		}
		else
		{
			POIManager.Instance.SetCutsceneCameraAngleActive(true);
		}

        GirlsManager.Instance.SetGirlsBlind(true);
        GirlsManager.Instance.ClearSpotted();

        float distanceInBuilding = 5f;
        float distance = 0.5f + 2f * distanceInBuilding + Vector3.Distance(GameManager.Instance.Player.CachedTransform.position, m_ConcertLocation.MarkerSpawnPoint.position);
        m_PlayerSpeed = distance / CONCERT_DURATION;

        m_PlayerPoints[0] = m_ConcertLocation.MarkerSpawnPoint.position;
        m_PlayerPoints[1] = m_PlayerPoints[0] + distanceInBuilding * m_ConcertLocation.MarkerSpawnPoint.forward;
        m_PlayerPoints[2] = m_PlayerPoints[0];

        m_PlayerPoints[0].y = 0f;
        m_PlayerPoints[1].y = 0f;
        m_PlayerPoints[2].y = 0f;

        m_PlayerPointID = 0;

        GameManager.Instance.Player.LocomotionActive = false;
        m_AudioVolume = m_ConcertLocation.Bejbe.volume;
        m_ConcertLocation.Bejbe.Play();
    }

    protected override void HandleUpdate()
	{
		base.HandleUpdate();

		m_Timer += Time.deltaTime;
        UpdatePlayerPosition();

        const float p = 0.4f;
        float x = (2f * (m_Timer / CONCERT_DURATION) - 1f);
        float bejbeFade = Mathf.Clamp01( 1.5f * (-(x * x) + 1f) );
        float fade = 1f - (m_Timer - CONCERT_DURATION * (1f - p)) / (CONCERT_DURATION * p);
        if (m_ConcertLocation)
        {
            m_ConcertLocation.Bejbe.volume = bejbeFade;
            m_ConcertLocation.Loop.volume = fade;
        }

        if (m_Timer > CONCERT_DURATION)
		{
			OnConcertFinished.Invoke();
		}
	}

	protected override void HandleLeave(AState nextState)
	{
		m_Mgr.SetGameTimerPause(false);
		m_Mgr.Player.SetInputLock(false);

		if (m_ConcertLocation.Type == ELocationType.Back)
		{
			POIManager.Instance.SetCutsceneCameraBackActive(false);
		}
		else
		{
			POIManager.Instance.SetCutsceneCameraAngleActive(false);
		}

		POIManager.Instance.GenerateMission(m_ConcertLocation.ID);
        GirlsManager.Instance.SetGirlsBlind(false);
        GirlsManager.Instance.SpawnGirl(5);
        GameManager.Instance.Player.LocomotionActive = true;
        m_ConcertLocation.Bejbe.Stop();
        m_ConcertLocation.Bejbe.volume = m_AudioVolume;

        m_ConcertLocation.Loop.Stop();
        m_ConcertLocation.Loop.volume = m_AudioVolume;

        m_ConcertLocation = null;
    }

    public void SetLocation(Location location)
	{
		m_ConcertLocation = location;
	}

    private void UpdatePlayerPosition()
    {
        float rotateSpeed = 200f;
        Vector3 nextPoint = m_PlayerPoints[m_PlayerPointID];
        Vector3 playerPosition = GameManager.Instance.Player.CachedTransform.position;
        float playerY = playerPosition.y;
        playerPosition.y = 0f;

        float distance = Vector3.Distance(playerPosition, nextPoint);
        float offset = m_PlayerSpeed * Time.deltaTime;

        Vector3 dir = (nextPoint - playerPosition).normalized;
        playerPosition += Mathf.Min(distance, offset) * dir;
        if ( distance < offset )
        {
            if ( m_PlayerPointID + 1 < m_PlayerPoints.Length )
            {
                ++m_PlayerPointID;
            }
        }


        float t = Time.deltaTime * rotateSpeed / Vector3.Angle(GameManager.Instance.Player.Input.DirectionVector, dir);
        Vector3 lookVec = Vector3.Slerp(GameManager.Instance.Player.Input.DirectionVector, dir, t);

        playerPosition.y = playerY;
        GameManager.Instance.Player.CachedTransform.position = playerPosition;
        GameManager.Instance.Player.Input.DirectionVector = lookVec;
    }
}
