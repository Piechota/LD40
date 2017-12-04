using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlsManager : ASingleton<GirlsManager>
{
	public GirlFOV FieldOfView;
	public GameObject GirlPrefab;
    [SerializeField]
    private int SpawnOnAwakeNum = 10;

    private List<GirlAI> m_Girls = new List<GirlAI>();
    private List<GirlAI> m_SpottedFans = new List<GirlAI>();

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
                m_Girls.Add(girl);
            }
        }
	}

	private void CheckPlayerExposed()
	{
		GameManager.Instance.Player.Animation.SetExposed(0 < m_SpottedFans.Count);
	}

    public void SetSpotted( GirlAI girl )
    {
        m_SpottedFans.Add(girl);
    }
    public void RemoveSpotted( GirlAI girl )
    {
        m_SpottedFans.Remove(girl);
    }

    public void ClearSpotted()
    {
        Transform worldBox = GameManager.Instance.WorldBox.transform;
        Vector3 worldBoxScale = worldBox.localScale;
        Vector3 worldBoxPosition = worldBox.position;
        Vector3 offset = Vector3.zero;
        NavMeshHit hitInfo;
        int spottedCount = m_SpottedFans.Count;
        for (int i = 0; i < spottedCount; ++i)
        {
            GirlAI girl = m_SpottedFans[i];
            offset.x = worldBoxScale.x * (0.5f * (Random.value * 2f - 1f));
            offset.z = worldBoxScale.z * (0.5f * (Random.value * 2f - 1f));

            Vector3 spawnPosition = worldBoxPosition + offset;
            if (NavMesh.SamplePosition(spawnPosition, out hitInfo, 100f, NavMesh.AllAreas))
            {
                girl.SetRoamingState();
                girl.SetTargetDestination(hitInfo.position);
            }
        }
        m_SpottedFans.Clear();
    }

    public void SetGirlsBlind( bool value )
    {
        int girlsCount = m_Girls.Count;
        for ( int i = 0; i < girlsCount; ++i)
        {
            m_Girls[i].Blind = value;
        }
    }
}
