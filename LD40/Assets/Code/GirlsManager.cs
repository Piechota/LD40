using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlsManager : ASingleton<GirlsManager>
{
    private List<SpawnPoint>[] m_SpawnPoints;
    private List<GirlAI> m_GirlsPool;
    public GirlFOV FieldOfView;
    public GameObject GirlPrefab;
    public float SpawnDelay = 2f;
    private float CurrentSpawnTime;

    public GirlsManager()
    {
        m_SpawnPoints = new List<SpawnPoint>[SpawnPoint.TagsNum];
        m_SpawnPoints[0] = new List<SpawnPoint>();
        m_GirlsPool = new List<GirlAI>();
    }

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        m_SpawnPoints[0].Add( spawnPoint );
        int tagsNum = spawnPoint.Tags.Length;
        for ( int i = 0; i < tagsNum; ++i )
        {
            int tag = (int)spawnPoint.Tags[i];
            if ( m_SpawnPoints[tag] == null )
            {
                m_SpawnPoints[tag] = new List<SpawnPoint>();
            }
            m_SpawnPoints[tag].Add( spawnPoint );
        }
    }

    void Start()
    {
        CurrentSpawnTime = SpawnDelay;
    }

    void Update()
    {
        CurrentSpawnTime -= GameManager.Instance.DeltaTime;
        if (CurrentSpawnTime < 0f)
        {
            SpawnGirl(null);
            CurrentSpawnTime = SpawnDelay;
        }
    }

    private void GenerateMission( GirlAI girl, SpawnPoint spawnPoint )
    {
        List<SpawnPoint> destinationPoints = null;

        int index = Random.Range(0, SpawnPoint.TagsNum);
        SpawnPoint.ETag tag = (SpawnPoint.ETag)index;
        List<SpawnPoint> spawnPoints = m_SpawnPoints[index];
        if (spawnPoints == null)
        {
            for (int i = 1; i < SpawnPoint.TagsNum; ++i)
            {
                spawnPoints = m_SpawnPoints[(index + i) % SpawnPoint.TagsNum];
                if ( spawnPoint != null)
                {
                    break;
                }
            }
        }
        if (spawnPoints != null)
        {
            int tagsNum = spawnPoint.Tags.Length;
            bool needCheck = false;
            for ( int i = 0; i < tagsNum; ++i)
            {
                if (spawnPoint.Tags[i] == tag)
                {
                    needCheck = true;
                    break;
                }
            }

            if ( needCheck )
            {
                destinationPoints = new List<SpawnPoint>();
                int spawnPointsNum = spawnPoints.Count;
                destinationPoints.Capacity = spawnPointsNum;
                for ( int i = 0; i < spawnPointsNum; ++i)
                {
                    if ( spawnPoint != spawnPoints[i] )
                    {
                        destinationPoints.Add(spawnPoints[i]);
                    }
                }
            }
            else
            {
                destinationPoints = spawnPoints;
            }
            girl.Initialize(spawnPoint, destinationPoints);
        }
    }

    public void SpawnGirl(GirlAI girl)
    {
        List < SpawnPoint > allSpawnPoints = m_SpawnPoints[0];
        int spawnPointsNum = allSpawnPoints.Count;
        if (0 < spawnPointsNum)
        {
            int index = Random.Range(0, spawnPointsNum);

            while (index < spawnPointsNum && allSpawnPoints[index].IsUsed == true )
            {
                ++index;
            }

            if (index < spawnPointsNum)
            {
                Vector3 spawnPosition = allSpawnPoints[index].CachedTransform.position;
                spawnPosition.y = 0f;

                NavMeshHit hitInfo;
                if (NavMesh.SamplePosition(spawnPosition, out hitInfo, 2f, NavMesh.AllAreas))
                {
                    if (girl == null)
                    {
                        girl = Instantiate(GirlPrefab, hitInfo.position, allSpawnPoints[index].CachedTransform.rotation).GetComponent<GirlAI>();
                    }
                    else
                    {
                        girl.CachedTransform.position = hitInfo.position;
                        girl.CachedTransform.rotation = allSpawnPoints[index].CachedTransform.rotation;
                    }
                    GenerateMission(girl, allSpawnPoints[index]);
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
