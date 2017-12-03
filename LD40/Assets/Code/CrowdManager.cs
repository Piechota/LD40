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

    [SerializeField]
    private float m_MaxWaitTime;
    [SerializeField]
    private int m_MaxWayPoints;
    [SerializeField]
    private int m_MaxNPC;
    private int m_CurrentNPCNum = 0;

    public void AddNPCToPool( NPC npc )
    {
        m_NPCPool.Add(npc);
        npc.gameObject.SetActive(false);
        --m_CurrentNPCNum;
    }

    private bool RandomNavMeshPoint( int areaMask, out Vector3 point)
    {
        Vector3 offset = m_WorldBox.localScale;
        offset.x *= 0.5f * (Random.value * 2f - 1f);
        offset.z *= 0.5f * (Random.value * 2f - 1f);
        offset.y = 0f;

        Vector3 position = m_WorldBox.position + offset;
        NavMeshHit hitInfo;
        if (NavMesh.SamplePosition(position, out hitInfo, 100f, areaMask))
        {
            point = hitInfo.position;
            return true; 
        }
        point = Vector3.one;
        return false;
    }
    private void SpawnNPC()
    {
        if (m_MaxNPC <= m_CurrentNPCNum)
        {
            return;
        }
        Vector3 navMeshPoint;
        float waitTime = 0f;

        if ( RandomNavMeshPoint(8, out navMeshPoint ) )
        {
            List<NPC.WayPoint> wayPoints = new List<NPC.WayPoint>();
            wayPoints.Add(new NPC.WayPoint( navMeshPoint, waitTime ) );

            int wayPointsNum = Random.Range(0, m_MaxWayPoints);
            for ( int i = 0; i < wayPointsNum; ++i)
            {
                if (RandomNavMeshPoint(NavMesh.AllAreas, out navMeshPoint))
                {
                    waitTime = Random.value * m_MaxWaitTime;
                    wayPoints.Add(new NPC.WayPoint(navMeshPoint, waitTime));
                }
            }

            if (RandomNavMeshPoint(8, out navMeshPoint))
            {
                wayPoints.Add(new NPC.WayPoint(navMeshPoint, 0f));
            }
            else
            {
                wayPoints.Add(wayPoints[0]);
            }

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
            npc.Init(wayPoints);
        }
    }

    public CrowdManager()
    {
        m_NPCPool = new List<NPC>();
    }

float time = -1f;
    void Update()
    {
        time -= Time.deltaTime;
        if ( time < 0f )
        {
            SpawnNPC();
            time = 5f;
        }
    }
}
