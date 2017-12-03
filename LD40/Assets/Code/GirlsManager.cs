using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlsManager : ASingleton<GirlsManager>
{
	public GirlFOV FieldOfView;
	public GameObject GirlPrefab;
    [SerializeField]
    private int SpawnOnAwakeNum = 10;

	private List<GirlAI> m_ActiveFans = new List<GirlAI>();

	private void Awake()
	{
        SpawnGirl(SpawnOnAwakeNum);
	}

	private void Update()
	{
		CheckPlayerExposed();
	}

	public void SpawnGirl(int spawnNum)
	{
        Transform worldBox = GameManager.Instance.WorldBox.transform;
        Vector3 worldBoxScale = worldBox.localScale;
        Vector3 worldBoxPosition = worldBox.position;
        Vector3 offset = Vector3.zero;
        NavMeshHit hitInfo;
        for (int i = 0; i < spawnNum; ++i)
        {
            offset.x = worldBoxScale.x * ( 0.5f * (Random.value * 2f - 1f) );
            offset.z = worldBoxScale.z * ( 0.5f * (Random.value * 2f - 1f) );

            Vector3 spawnPosition = worldBoxPosition + offset;
            if (NavMesh.SamplePosition(spawnPosition, out hitInfo, 2f, NavMesh.AllAreas))
            {
                GirlAI girl = Instantiate(GirlPrefab, hitInfo.position, Random.rotation).GetComponent<GirlAI>();
                girl.Initialize();
                m_ActiveFans.Add(girl);
            }
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
