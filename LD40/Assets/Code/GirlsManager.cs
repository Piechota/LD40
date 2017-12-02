using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlsManager : ASingleton<GirlsManager>
{
    private List<SpawnPoint> m_SpawnPoints;
    private List<GirlAI> m_GirlsPool;
    public GirlFOV FieldOfView;
    public GameObject GirlPrefab;
    public float SpawnDelay = 2f;
    private float CurrentSpawnTime;

    public GirlsManager()
    {
        m_SpawnPoints = new List<SpawnPoint>();
        m_GirlsPool = new List<GirlAI>();
    }

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        m_SpawnPoints.Add(spawnPoint);
    }

    void Start()
    {
        CurrentSpawnTime = SpawnDelay;
    }

    void Update()
    {
        CurrentSpawnTime -= Time.deltaTime;
        if (CurrentSpawnTime < 0f)
        {
            SpawnGirl(null);
            CurrentSpawnTime = SpawnDelay;
        }
    }

    public void SpawnGirl(GirlAI girl)
    {
        int spawnPointsNum = m_SpawnPoints.Count;
        if (0 < spawnPointsNum)
        {
            int index = Random.Range(0, spawnPointsNum);

            while (index < spawnPointsNum && m_SpawnPoints[index].IsUsed == true )
            {
                ++index;
            }

            if (index < spawnPointsNum)
            {
                Vector3 spawnPosition = m_SpawnPoints[index].CachedTransform.position;
                spawnPosition.y = 0f;

                NavMeshHit hitInfo;
                if (NavMesh.SamplePosition(spawnPosition, out hitInfo, 2f, NavMesh.AllAreas))
                {
                    if (girl == null)
                    {
                        girl = Instantiate(GirlPrefab, hitInfo.position, m_SpawnPoints[index].CachedTransform.rotation).GetComponent<GirlAI>();
                    }
                    else
                    {
                        girl.CachedTransform.position = hitInfo.position;
                        girl.CachedTransform.rotation = m_SpawnPoints[index].CachedTransform.rotation;
                    }
		    girl.Initialize(m_SpawnPoints[index]);
                    m_SpawnPoints[index].IsUsed = true;

					UIManager.Instance.CreatePartnerTimer(girl);
                }
            }
        }
    }

    public bool SpawnGirlFromPool()
    {
        int girlPoolNum = m_GirlsPool.Count;
        if ( 0 < girlPoolNum )
        {
            int index = Random.Range(0, girlPoolNum);
            while (index < girlPoolNum && m_GirlsPool[index].gameObject.activeSelf == false)
            {
                ++index;
            }

            if (index < girlPoolNum)
            {
                SpawnGirl(m_GirlsPool[index]);
                m_GirlsPool[index].gameObject.SetActive( true );
                return true;
            }
        }

        return false;
    }

    public void AddGirlToPool( GirlAI girl )
    {
        m_GirlsPool.Add(girl);
        girl.gameObject.SetActive( false );
    }
}
