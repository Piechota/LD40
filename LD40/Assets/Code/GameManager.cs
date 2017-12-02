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
}
