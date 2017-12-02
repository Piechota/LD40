using UnityEngine;
using UnityEngine.SceneManagement;

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

	public bool IsGameTimerPaused { get; private set; }
	public float DeltaTime { get; private set; }			// these timers are using the in-game clock
	public float FixedDeltaTime { get; private set; }

	private void Awake()
	{
		IsGameOver = false;
		IsGameTimerPaused = false;
	}

	private void Update()
	{
		bool shouldUpdateTimer = !IsGameTimerPaused && !IsGameOver;
		DeltaTime = shouldUpdateTimer ? Time.deltaTime : 0;
		FixedDeltaTime = shouldUpdateTimer ? Time.fixedDeltaTime : 0;

		if (IsGameOver && Input.anyKey)
		{
			Restart();
		}
	}

	public void SetGameOver()
	{
		IsGameOver = true;
		IsGameTimerPaused = true;

		Player.SetInputLock(true);
		OnGameOver.Invoke();
	}

	private void Restart()
	{
		SceneManager.LoadScene(0);
	}
}
