using UnityEngine;

public class GameManager : ASingleton<GameManager>
{
	public AEvent OnGameOver = new AEvent();

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

	public bool IsGameOver { get; private set; }

	public void SetGameOver()
	{
		IsGameOver = true;
		Player.SetInputLock(true);
		OnGameOver.Invoke();
	}
}
