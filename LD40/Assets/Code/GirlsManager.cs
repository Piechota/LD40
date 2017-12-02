using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlsManager : ASingleton<GirlsManager>
{
    private List<SpawnPoint> m_SpawnPoints;
    public GirlFOV FieldOfView;
    public GameObject GirlPrefab;
    public float SpawnDelay = 2f;
    private float CurrentSpawnTime;

    public GirlsManager()
    {
        m_SpawnPoints = new List<SpawnPoint>();
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
            SpawnGirl();
            CurrentSpawnTime = SpawnDelay;
        }
    }

    public void SpawnGirl()
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
                    GirlAI girl = Instantiate(GirlPrefab, hitInfo.position, m_SpawnPoints[index].CachedTransform.rotation).GetComponent<GirlAI>();
					girl.Initialize(m_SpawnPoints[index]);
                    m_SpawnPoints[index].IsUsed = true;

					UIManager.Instance.CreatePartnerTimer(girl);
                }
            }
        }
    }
}
