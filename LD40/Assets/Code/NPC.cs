using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC : CachedMonoBehaviour {
    public struct WayPoint
    {
        public Vector3 Position;
        public float WaitTime;

        public WayPoint(Vector3 position, float waitTime)
        {
            Position = position;
            WaitTime = waitTime;
        }
    }
    private List<WayPoint> m_WayPoints;
    private int m_CurrentWaypoint;
    private float m_WaitTime;
    private NavMeshAgent m_Agent;
    public void Init( List<WayPoint> wayPoints )
    {
        m_WayPoints = wayPoints;
        CachedTransform.position = wayPoints[0].Position;
        m_CurrentWaypoint = 0;
        m_WaitTime = -1f;
    }
    // Use this for initialization
    void Start () {
        m_Agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if ( m_Agent.remainingDistance < 0.1f)
        {
            m_WaitTime -= GameManager.Instance.DeltaTime;
            if ( m_WaitTime < 0f )
            {
                do
                {
                    ++m_CurrentWaypoint;
                    if (m_CurrentWaypoint < m_WayPoints.Count)
                    {
                        m_WaitTime = m_WayPoints[m_CurrentWaypoint].WaitTime;
                    }
                    else
                    {
                        CrowdManager.Instance.AddNPCToPool(this);
                        return;
                    }
                } while (!m_Agent.SetDestination(m_WayPoints[m_CurrentWaypoint].Position));
            }
        }
	}
}
