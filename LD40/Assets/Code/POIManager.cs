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

	private List<SpawnPoint>[] m_SpawnPoints = new List<SpawnPoint>[SpawnPoint.TagsNum];
	private List<NPCPoint> m_NPCPoints = new List<NPCPoint>();

	public POIManager()
	{
		m_SpawnPoints[0] = new List<SpawnPoint>();
	}

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

	public void RegisterSpawnPoint(SpawnPoint spawnPoint)
	{
		m_SpawnPoints[0].Add(spawnPoint);
		int tagsNum = spawnPoint.Tags.Length;
		for (int i = 0; i < tagsNum; ++i)
		{
			int tag = (int)spawnPoint.Tags[i];
			if (m_SpawnPoints[tag] == null)
			{
				m_SpawnPoints[tag] = new List<SpawnPoint>();
			}
			m_SpawnPoints[tag].Add(spawnPoint);
		}
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

	public SpawnPoint GetFreeSpawnPoint()
	{
		List<SpawnPoint> allSpawnPoints = m_SpawnPoints[0];
		int spawnPointsNum = allSpawnPoints.Count;
		if (0 < spawnPointsNum)
		{
			int index = Random.Range(0, spawnPointsNum);

			while (index < spawnPointsNum && allSpawnPoints[index].IsUsed == true)
			{
				++index;
			}

			if (index < spawnPointsNum)
			{
				return allSpawnPoints[index];
			}
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
