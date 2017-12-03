using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlsManager : ASingleton<GirlsManager>
{
	public GirlFOV FieldOfView;
	public GameObject GirlPrefab;
	public float SpawnDelay = 2f;
	private float CurrentSpawnTime;

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
			SpawnGirl();
			CurrentSpawnTime = SpawnDelay;
		}

		CheckPlayerExposed();
	}

	private void SpawnGirl()
	{
        Transform worldBox = GameManager.Instance.WorldBox.transform;
        Vector3 offset = worldBox.localScale;
        offset.y = 0f;
        offset.x *= 0.5f * Random.value;
        offset.z *= 0.5f * Random.value;

		Vector3 spawnPosition = worldBox.position + offset;
		NavMeshHit hitInfo;
		if (NavMesh.SamplePosition(spawnPosition, out hitInfo, 2f, NavMesh.AllAreas))
		{
			GirlAI girl = Instantiate(GirlPrefab, hitInfo.position, Random.rotation).GetComponent<GirlAI>();
			girl.Initialize();
			m_ActiveFans.Add(girl);
		}
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
