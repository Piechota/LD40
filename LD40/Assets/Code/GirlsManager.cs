﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class FieldOfView
{
    public int RaysNum;
    public float RaysDistance;
    public float ConeDegree;

    public bool TestCollision(Vector3 position, Vector3 lookDir)
    {
        if (GameManager.Instance.PlayerCollider != null)
        {
            float playerRadius = GameManager.Instance.PlayerCollider.radius;
            float coneDot = Mathf.Cos(ConeDegree * Mathf.Deg2Rad);
            Vector3 playerPosition = GameManager.Instance.Player.transform.position;
            Vector3 dir = playerPosition - position;
            if (Vector3.Distance(playerPosition - dir.normalized * playerRadius, position) <= RaysDistance)
            {
                Vector3 perpendicularDir = new Vector3(dir.z, 0.0f, -dir.x);
                perpendicularDir.Normalize();
                Vector3 testPosition = playerPosition + perpendicularDir * playerRadius;
                RaycastHit hitInfo;
                float delta = (2.0f * playerRadius) / (float)RaysNum;
                for (int i = 0; i < RaysNum; ++i)
                {
                    dir = testPosition - position;
                    dir.Normalize();

                    if (coneDot < Vector3.Dot(dir, lookDir))
                    {
                        if (Physics.Raycast(position, dir, out hitInfo, RaysDistance))
                        {
                            if (hitInfo.transform.gameObject.layer == GameManager.Instance.Player.gameObject.layer)
                            {
                                return true;
                            }
                        }
                    }
                    testPosition -= perpendicularDir * delta;
                }
            }
        }
        return false;
    }
}

public class GirlsManager : ASingleton<GirlsManager>
{
    private List<SpawnPoint> m_SpawnPoints;
    private List<GirlAI> m_GirlsPool;

    public FieldOfView FieldOfView;
    public GameObject GirlPrefab;
    public float SpawnDelay = 2f;
    private float CurrentSpawnTime;
    public GirlsManager()
    {
        m_SpawnPoints = new List<SpawnPoint>();
        m_GirlsPool = new List<GirlAI>();
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
            SpawnGirl(null);
            CurrentSpawnTime = SpawnDelay;
        }
    }

    public void SpawnGirl(GirlAI girl)
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
                    if (girl == null)
                    {
                        girl = Instantiate(GirlPrefab, hitInfo.position, m_SpawnPoints[index].CachedTransform.rotation).GetComponent<GirlAI>();
                    }
                    else
                    {
                        girl.CachedTransform.position = hitInfo.position;
                        girl.CachedTransform.rotation = m_SpawnPoints[index].CachedTransform.rotation;
                    }
                    m_SpawnPoints[index].IsUsed = true;
                    girl.OriginPoint = m_SpawnPoints[index];
                }
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
                m_GirlsPool[index].gameObject.SetActive( true );
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
