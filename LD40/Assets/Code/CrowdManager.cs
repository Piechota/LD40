using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdManager : ASingleton<CrowdManager>
{
    private List<NPC> m_NPCPool;

    [SerializeField]
    private GameObject m_NPCPrefab;
    [SerializeField]
    private Transform m_WorldBox;

    public float LastDistanceTime;
    public float LastDistanceOffset;
    public Vector2 MaxWaitTime;
    [SerializeField]
    private int m_MaxNPC;
    private int m_CurrentNPCNum = 0;
    [SerializeField]
    private Vector2 m_DistanceFromPlayer;

    public void AddNPCToPool( NPC npc )
    {
        m_NPCPool.Add(npc);
        npc.gameObject.SetActive(false);
        --m_CurrentNPCNum;
    }

    public void SpawnNPC()
    {
        if (m_MaxNPC <= m_CurrentNPCNum)
        {
            return;
        }

        Vector3 dir = Random.onUnitSphere;
        dir.y = 0f;
        dir.Normalize();

        Vector3 position = GameManager.Instance.Player.CachedTransform.position + dir * ( m_DistanceFromPlayer.x + Random.value * (m_DistanceFromPlayer.y - m_DistanceFromPlayer.x) );

        NPC npc = null;
        int poolSize = m_NPCPool.Count;
        if ( 0 < poolSize )
        {
            int index = Random.Range(0, poolSize);
            npc = m_NPCPool[index];
            m_NPCPool[index] = m_NPCPool[poolSize - 1];
            m_NPCPool.RemoveAt(poolSize - 1);

            npc.gameObject.SetActive(true);
        }

        if ( npc == null )
        {
            npc = Instantiate(m_NPCPrefab).GetComponent<NPC>();
        }

        ++m_CurrentNPCNum;
        npc.Init(position);
    }

    public CrowdManager()
    {
        m_NPCPool = new List<NPC>();
    }
    void Start()
    {
        for ( int i = 0; i < m_MaxNPC; ++i)
        {
            SpawnNPC();
        }
    }
}
