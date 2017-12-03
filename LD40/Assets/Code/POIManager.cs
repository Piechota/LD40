using System.Collections.Generic;
using UnityEngine;

public class POIManager : ASingleton<POIManager>
{
	public readonly AEvent<Location> OnMissionStarted = new AEvent<Location>();
	public readonly AEvent OnMissionCompleted = new AEvent();

	public Location TargetLocation { get; private set; }
	private List<Location> m_Locations = new List<Location>();

	private List<NPCPoint> m_NPCPoints = new List<NPCPoint>();

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
