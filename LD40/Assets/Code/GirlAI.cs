using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class GirlAI : CachedMonoBehaviour
{
    public Material ConeMaterial;
    private bool m_Catched;
    public bool IsFollowing;

    private NavMeshAgent m_Agent;
    [Header("Following")]
    [SerializeField]
    private float m_FollowingDistance;

    // Use this for initialization
    void Start() {

    }

    private void DrawCone( Color color )
    {
        FieldOfView fieldOfView = GirlsManager.Instance.FieldOfView;
        const int triangleNum = 20;

        float deltaDegree = 2f * fieldOfView.ConeDegree * Mathf.Deg2Rad / (float)triangleNum;
        float cosDelta = Mathf.Cos(deltaDegree);
        float sinDelta = Mathf.Sin(deltaDegree);

        float coneCos = Mathf.Cos(-fieldOfView.ConeDegree * Mathf.Deg2Rad);
        float coneSin = Mathf.Sin(-fieldOfView.ConeDegree * Mathf.Deg2Rad);

        Vector3 initDir = new Vector3(coneSin * CachedTransform.forward.z + coneCos * CachedTransform.forward.x, 0f, coneCos * CachedTransform.forward.z - coneSin * CachedTransform.forward.x);
        initDir.Normalize();
        Vector3 pos0 = CachedTransform.position;
        float coneRadius = fieldOfView.RaysDistance;
        GL.Begin(GL.TRIANGLES);
        ConeMaterial.SetPass(0);
        ConeMaterial.color = color;
        GL.Color(Color.white);

        for (int i = 0; i < triangleNum; ++i)
        {
            GL.Vertex3(pos0.x, pos0.y, pos0.z);
            GL.Vertex3(pos0.x + initDir.x * coneRadius, pos0.y + initDir.y * coneRadius, pos0.z + initDir.z * coneRadius);

            float x = sinDelta * initDir.z + cosDelta * initDir.x;
            float z = cosDelta * initDir.z - sinDelta * initDir.x;
            initDir.x = x;
            initDir.z = z;

            GL.Vertex3(pos0.x + initDir.x * coneRadius, pos0.y + initDir.y * coneRadius, pos0.z + initDir.z * coneRadius);
        }
        GL.End();
    }

    private void FollowPlayer()
    {

    }

    void OnRenderObject()
    {
        DrawCone(m_Catched ? Color.red : Color.blue);
    }
    // Update is called once per frame
    void Update ()
    {
        if (!IsFollowing)
        {
            m_Catched = GirlsManager.Instance.FieldOfView.TestCollision(CachedTransform.position, CachedTransform.forward);
        }
        else
        {
            FollowPlayer(); 
        }
	}

    public void StopFollowing()
    {
        IsFollowing = false;
        m_Agent.isStopped = true;
    }

    public void StartFollowing()
    {
        IsFollowing = true;
        m_Agent.isStopped = false;
    }
}
