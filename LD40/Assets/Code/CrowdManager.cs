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
    [SerializeField]
    private float m_DistanceFromPlayer;

    public void SpawnNPC(int spawnNum)
    {
        Vector3 playerPosition = -GameManager.Instance.Player.CachedTransform.position;
        Vector3 dir = Vector3.zero;
        float cameraY = Camera.main.transform.position.y;
        for (int i = 0; i < spawnNum; ++i)
        {
            Vector2 offset = Random.insideUnitCircle;
            offset.Normalize();
            dir.x = offset.x;
            dir.y = offset.y * Camera.main.aspect;
            dir.z = cameraY;

            Vector3 positionWS = Camera.main.ViewportToWorldPoint(dir);
            positionWS.y = 0f;

            Vector3 position = positionWS + (Random.value * m_DistanceFromPlayer) * (positionWS + playerPosition).normalized;
            NPC npc = Instantiate(m_NPCPrefab).GetComponent<NPC>();

            npc.Init(position);
        }
    }

    void Start()
    {
        SpawnNPC(m_StartNPCNum);
    }
}
