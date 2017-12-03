using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC : CachedMonoBehaviour {
    [SerializeField]
    private GameObject[] m_Meshes;

    private float m_WaitTime = -1f;
    private NavMeshAgent m_Agent;
    private float m_LastDistance;
    private float m_LastDistanceTime;
    // Use this for initialization
    void Start () {
        m_Agent = GetComponent<NavMeshAgent>();
        Instantiate(m_Meshes[Random.Range(0, m_Meshes.Length)], CachedTransform);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if ( m_Agent.remainingDistance < 0.1f )
        {
            m_WaitTime -= GameManager.Instance.DeltaTime;
            if ( m_WaitTime < 0f )
            {
                NewDestination();
            }
        }
        else
        {
            if ( Mathf.Abs(m_Agent.remainingDistance - m_LastDistance ) < CrowdManager.Instance.LastDistanceOffset )
            {
                m_LastDistanceTime -= GameManager.Instance.DeltaTime;
                if (m_LastDistanceTime < 0f)
                {
                    NewDestination();
                }
            }
            else
            {
                m_LastDistanceTime = CrowdManager.Instance.LastDistanceTime;
            }

            m_LastDistance = m_Agent.remainingDistance;
        }
	}

    private void NewDestination()
    {
        NPCPoint npcPoint = POIManager.Instance.GetRandomNPCPoint();
        if (npcPoint)
        {
            m_Agent.SetDestination(npcPoint.CachedTransform.position);
            m_WaitTime = CrowdManager.Instance.MaxWaitTime.x + Random.value * (CrowdManager.Instance.MaxWaitTime.y - CrowdManager.Instance.MaxWaitTime.x);
            m_LastDistance = m_Agent.remainingDistance;
        }
    }
}
