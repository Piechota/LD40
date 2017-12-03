using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdManager : ASingleton<CrowdManager>
{
    [SerializeField]
    private GameObject m_NPCPrefab;
    [SerializeField]
    private Transform m_WorldBox;

    public float LastDistanceTime;
    public float LastDistanceOffset;
    public Vector2 MaxWaitTime;
    [SerializeField]
    private int m_StartNPCNum;
    [SerializeField]
    private Vector2 m_DistanceFromPlayer;

    public void SpawnNPC(int spawnNum)
    {
        Vector3 playerPosition = GameManager.Instance.Player.CachedTransform.position;
        float distanceSpawnDelta = m_DistanceFromPlayer.y - m_DistanceFromPlayer.x;
        for (int i = 0; i < spawnNum; ++i)
        {
            Vector3 dir = Random.onUnitSphere;
            dir.y = 0f;
            dir.Normalize();

            Vector3 position = playerPosition + dir * (m_DistanceFromPlayer.x + Random.value * distanceSpawnDelta);
            NPC npc = Instantiate(m_NPCPrefab).GetComponent<NPC>();

            npc.Init(position);
        }
    }

    void Start()
    {
        SpawnNPC(m_StartNPCNum);
    }
}
