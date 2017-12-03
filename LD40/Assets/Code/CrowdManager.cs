using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdManager : ASingleton<CrowdManager>
{
    [SerializeField]
    private GameObject m_NPCPrefab;

    public float LastDistanceTime;
    public float LastDistanceOffset;
    public Vector2 MaxWaitTime;
    [SerializeField]
    private int m_StartNPCNum;

    public void SpawnNPC(int spawnNum)
    {
        Transform worldBox = GameManager.Instance.WorldBox.transform;
        Vector3 worldBoxScale = worldBox.localScale;
        Vector3 worldBoxPosition = worldBox.position;
        Vector3 offset = Vector3.zero;
        NavMeshHit hitInfo;
        for (int i = 0; i < spawnNum; ++i)
        {
            offset.x = worldBoxScale.x * (0.5f * (Random.value * 2f - 1f));
            offset.z = worldBoxScale.z * (0.5f * (Random.value * 2f - 1f));

            Vector3 spawnPosition = worldBoxPosition + offset;
            if (NavMesh.SamplePosition(spawnPosition, out hitInfo, 2f, NavMesh.AllAreas))
            {
                Instantiate(m_NPCPrefab, hitInfo.position, Random.rotation);
            }
        }
    }

    void Start()
    {
        SpawnNPC(m_StartNPCNum);
    }
}
