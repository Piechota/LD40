using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlsManager : ASingleton<GirlsManager>
{
    private List<GirlAI> m_GirlsPool;
    public GirlFOV FieldOfView;
    public GameObject GirlPrefab;
    public float SpawnDelay = 2f;
    private float CurrentSpawnTime;

    public GirlsManager()
    {
        m_GirlsPool = new List<GirlAI>();
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
    public void SpawnGirl(GirlAI girl)
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
