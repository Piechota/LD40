using UnityEngine;

[System.Serializable]
public class FieldOfView
{
    public int RaysNum;
    public float RaysDistance;
    public float ConeDegree;

    public bool TestCollision(Vector3 position, Vector3 lookDir)
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

            float delta = (2.0f * playerRadius) / (float)RaysNum;
            for (int i = 0; i < RaysNum; ++i)
            {
                dir = testPosition - position;
                dir.Normalize();

                if (coneDot < Vector3.Dot(dir, lookDir))
                {
                    if (Physics.Raycast(position, dir, RaysDistance))
                    {
                        return true;
                    }
                }
                testPosition -= perpendicularDir * delta;
            }
        }
        return false;
    }
}

public class GameManager : ASingleton<GameManager>
{
    private PlayerController m_Player;
	public PlayerController Player
	{
		get
		{
			if (m_Player == null)
			{
				m_Player = FindObjectOfType<PlayerController>();
			}

			return m_Player;
		}
	}
    private CapsuleCollider m_PlayerCollider;
    public CapsuleCollider PlayerCollider
    {
        get
        {
            if (m_PlayerCollider == null)
            {
                m_PlayerCollider = Player.GetComponentInChildren<CapsuleCollider>();
            }

            return m_PlayerCollider;
        }
    }

    public FieldOfView FieldOfView;
}
