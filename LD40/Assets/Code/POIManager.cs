using System.Collections.Generic;
using UnityEngine;

public class POIManager : ASingleton<POIManager>
{
	public readonly AEvent<Location> OnMissionStarted = new AEvent<Location>();
	public readonly AEvent OnMissionCompleted = new AEvent();

	private List<Location> m_Locations = new List<Location>();
	public Location TargetLocation { get; private set; }
	private float m_MissionTimer = 0f;
	private float m_MissionDuration = 60f;
	public float MissionTimerValue { get { return m_MissionTimer / m_MissionDuration; } }

	private List<NPCPoint> m_NPCPoints = new List<NPCPoint>();

	private void Update()
	{
		if (TargetLocation != null)
		{
			m_MissionTimer -= GameManager.Instance.DeltaTime;
			if (m_MissionTimer <= 0)
			{
				GameManager.Instance.SetGameOver();
			}
		}
	}

	public void RegisterLocation(Location loc)
	{
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

	public void GenerateMission()
	{
		int rand = Random.Range(0, m_Locations.Count);
		TargetLocation = m_Locations[rand];
		TargetLocation.SetTarget(true);

		m_MissionTimer = m_MissionDuration;
		UIManager.Instance.ShowLocationMarker(TargetLocation);

		OnMissionStarted.Invoke(TargetLocation);
	}

	public void CompleteActiveMission()
	{
		TargetLocation.SetTarget(false);
		TargetLocation = null;
		OnMissionCompleted.Invoke();
		GenerateMission();
	}
}
