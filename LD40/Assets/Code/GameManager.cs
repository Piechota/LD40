using UnityEngine;

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
}
