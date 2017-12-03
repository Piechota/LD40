using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlsManager : ASingleton<GirlsManager>
{
	public GirlFOV FieldOfView;
	public GameObject GirlPrefab;
	public float SpawnDelay = 2f;
	private float CurrentSpawnTime;

	private List<GirlAI> m_FanPool = new List<GirlAI>();
	private List<GirlAI> m_ActiveFans = new List<GirlAI>();

	private void Awake()
	{
		CurrentSpawnTime = SpawnDelay;
	}

	private void Update()
	{
		CurrentSpawnTime -= GameManager.Instance.DeltaTime;
		if (CurrentSpawnTime < 0f)
		{
			SpawnGirl(null);
			CurrentSpawnTime = SpawnDelay;
		}

		CheckPlayerExposed();
	}

	public bool SpawnGirlFromPool()
	{
		int girlPoolNum = m_FanPool.Count;
		if (0 < girlPoolNum)
		{
			int index = Random.Range(0, girlPoolNum);
			while (index < girlPoolNum && m_FanPool[index].gameObject.activeSelf == false)
			{
				++index;
			}

			if (index < girlPoolNum)
			{
				SpawnGirl(m_FanPool[index]);
				return true;
			}
		}

		return false;
	}

	private void SpawnGirl(GirlAI girl)
	{
		SpawnPoint spawnPoint = POIManager.Instance.GetFreeSpawnPoint();

		if (spawnPoint != null)
		{
			Vector3 spawnPosition = spawnPoint.CachedTransform.position;
			spawnPosition.y = 0f;

			NavMeshHit hitInfo;
			if (NavMesh.SamplePosition(spawnPosition, out hitInfo, 2f, NavMesh.AllAreas))
			{
				if (girl == null)
				{
					girl = Instantiate(GirlPrefab, hitInfo.position, spawnPoint.CachedTransform.rotation).GetComponent<GirlAI>();
				}
				else
				{
					girl.CachedTransform.position = hitInfo.position;
					girl.CachedTransform.rotation = spawnPoint.CachedTransform.rotation;
				}
				girl.Initialize(spawnPoint);
				m_ActiveFans.Add(girl);
			}
		}
	}

	public void AddGirlToPool(GirlAI girl)
	{
		m_FanPool.Add(girl);
		m_ActiveFans.Remove(girl);
		girl.gameObject.SetActive(false);
	}

	private void CheckPlayerExposed()
	{
		bool result = false;
		for (int i = 0; i < m_ActiveFans.Count; ++i)
		{
			GirlAI fan = m_ActiveFans[i];
			if (fan != null && fan.HasSpotted)
			{
				result = true;
				break;
			}
		}

		GameManager.Instance.Player.Animation.SetExposed(result);
	}
}
