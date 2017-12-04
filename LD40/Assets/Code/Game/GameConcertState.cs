using UnityEngine;

public class GameConcertState : AGameState
{
	public readonly AEvent OnConcertFinished = new AEvent();

	private float m_Timer = 0f;
	private Location m_ConcertLocation;

	private const float CONCERT_DURATION = 4f;

	public GameConcertState() : base(EGameState.Concert)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Mgr.SetGameTimerPause(true);
		m_Mgr.Player.SetInputLock(true);

		m_Timer = 0f;
		POIManager.Instance.SetCutsceneCameraActive(true);
        GirlsManager.Instance.SetGirlsBlind(true);
        GirlsManager.Instance.ClearSpotted();
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();

		m_Timer += Time.deltaTime;
		if (m_Timer > CONCERT_DURATION)
		{
			OnConcertFinished.Invoke();
		}
	}

	protected override void HandleLeave(AState nextState)
	{
		m_ConcertLocation = null;
		m_Mgr.SetGameTimerPause(false);
		m_Mgr.Player.SetInputLock(false);
		POIManager.Instance.SetCutsceneCameraActive(false);
		POIManager.Instance.GenerateMission();
        GirlsManager.Instance.SetGirlsBlind(false);
        GirlsManager.Instance.SpawnGirl(5);
    }

    public void SetLocation(Location location)
	{
		m_ConcertLocation = location;
	}
}
