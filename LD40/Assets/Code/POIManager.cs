using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class POIManager : ASingleton<POIManager>
{
	public readonly AEvent<Location> OnMissionStarted = new AEvent<Location>();
	public readonly AEvent OnMissionCompleted = new AEvent();

	[SerializeField]
	private CinemachineVirtualCamera m_CutsceneCameraBack;
	[SerializeField]
	private CinemachineVirtualCamera m_CutsceneCameraAngle;

	private List<Location> m_Locations = new List<Location>();
	public Location TargetLocation { get; private set; }

	public float MissionTimer { get; private set; }
	private float m_MissionDuration = 60f;
	public float MissionTimerValue { get { return MissionTimer / m_MissionDuration; } }

	public int MissionCounter { get; private set; }

	private List<NPCPoint> m_NPCPoints = new List<NPCPoint>();

	private void Update()
	{
		if (TargetLocation != null)
		{
			MissionTimer -= GameManager.Instance.DeltaTime;
			if (MissionTimer <= 0)
			{
				GameManager.Instance.SetGameOver();
			}
		}
	}

	public void RegisterLocation(Location loc)
	{
        loc.ID = m_Locations.Count;
        m_Locations.Add(loc);
	}

	public void RegisterNPCPoint(NPCPoint npcPoint)
	{
		m_NPCPoints.Add(npcPoint);
	}

	public NPCPoint GetRandomNPCPoint()
	{
		if (0 < m_NPCPoints.Count)
		{
			return m_NPCPoints[Random.Range(0, m_NPCPoints.Count)];
		}
		return null;
	}

	public void GenerateMission( int lastLocation )
	{
		UIManager.Instance.UpdateMissionCounter(MissionCounter);
		int rand = Random.Range(0, m_Locations.Count);
        if (rand == lastLocation)
        {
            rand = (rand + 1) % m_Locations.Count;
        }

		TargetLocation = m_Locations[rand];
		TargetLocation.SetTarget(true);
        TargetLocation.Loop.Play();

        MissionTimer = m_MissionDuration;
		UIManager.Instance.ShowLocationMarker(TargetLocation);

		OnMissionStarted.Invoke(TargetLocation);
	}

	public void CompleteActiveMission()
	{
		++MissionCounter;

		UIManager.Instance.ShowLocationMarker(null);
		TargetLocation.SetTarget(false);

		OnMissionCompleted.Invoke();
		GameManager.Instance.StartConcert(TargetLocation);
		TargetLocation = null;
	}

	public void SetCutsceneCameraBackActive(bool set)
	{
		m_CutsceneCameraBack.gameObject.SetActive(set);
	}

	public void SetCutsceneCameraAngleActive(bool set)
	{
		m_CutsceneCameraAngle.gameObject.SetActive(set);
	}
}
