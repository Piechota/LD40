using System.Collections.Generic;
using UnityEngine;

public class POIManager : ASingleton<POIManager>
{
    private List<SpawnPoint>[] m_SpawnPoints;
    private List<NPCPoint> m_NPCPoints;

    public POIManager()
    {
        m_SpawnPoints = new List<SpawnPoint>[SpawnPoint.TagsNum];
        m_SpawnPoints[0] = new List<SpawnPoint>();
        m_NPCPoints = new List<NPCPoint>();
    }

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        m_SpawnPoints[0].Add(spawnPoint);
        int tagsNum = spawnPoint.Tags.Length;
        for (int i = 0; i < tagsNum; ++i)
        {
            int tag = (int)spawnPoint.Tags[i];
            if (m_SpawnPoints[tag] == null)
            {
                m_SpawnPoints[tag] = new List<SpawnPoint>();
            }
            m_SpawnPoints[tag].Add(spawnPoint);
        }
    }


    public void RegisterNPCPoint(NPCPoint npcPoint)
    {
        m_NPCPoints.Add(npcPoint);
    }

    public NPCPoint GetRandomNPCPoint()
    {
        if (0 < m_NPCPoints.Count)
        {
            return m_NPCPoints[Random.Range(0, m_NPCPoints.Count)];
        }
        return null;
    }

    public SpawnPoint GetFreeSpawnPoint()
    {
        List<SpawnPoint> allSpawnPoints = m_SpawnPoints[0];
        int spawnPointsNum = allSpawnPoints.Count;
        if (0 < spawnPointsNum)
        {
            int index = Random.Range(0, spawnPointsNum);

            while (index < spawnPointsNum && allSpawnPoints[index].IsUsed == true)
            {
                ++index;
            }

            if (index < spawnPointsNum)
            {
                return allSpawnPoints[index];
            }
        }

        return null;
    }
}
