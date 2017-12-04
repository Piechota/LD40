using UnityEngine;

public class GameManager : ASingleton<GameManager>
{
	public AEvent OnGameOver = new AEvent();
    public Transform WorldBox;
	public Transform Floor;

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

	private FiniteStateMachine m_FSM;
	public FiniteStateMachine FSM { get { return m_FSM; } }

	private GameDefaultState m_DefaultState;
	private GameConcertState m_ConcertState;
	private GameGameOverState m_GameOverState;

	private void Awake()
	{
		IsGameOver = false;
		IsGameTimerPaused = false;

		InitializeStateMachine();
	}

	private void Start()
	{
		POIManager.Instance.GenerateMission(-1);
	}

	private void Update()
	{
		bool shouldUpdateTimer = !IsGameTimerPaused && !IsGameOver;
		DeltaTime = shouldUpdateTimer ? Time.deltaTime : 0;
		FixedDeltaTime = shouldUpdateTimer ? Time.fixedDeltaTime : 0;

		m_FSM.Update();
	}

	private void FixedUpdate()
	{
		m_FSM.FixedUpdate();
	}

	public void StartConcert(Location location)
	{
		m_ConcertState.SetLocation(location);
		m_ConcertState.OnConcertFinished.AddListener(HandleConcertFinished);
		m_FSM.TransitionTo(m_ConcertState);
	}

	private void HandleConcertFinished()
	{
		m_ConcertState.OnConcertFinished.RemoveListener(HandleConcertFinished);
		m_FSM.TransitionTo(m_DefaultState);
	}

	public void SetGameOver()
	{
		m_FSM.TransitionTo(m_GameOverState);
		OnGameOver.Invoke();
	}

	public void SetGameTimerPause(bool set)
	{
		IsGameTimerPaused = set;
	}

	private void InitializeStateMachine()
	{
		m_FSM = new FiniteStateMachine();

		m_DefaultState = new GameDefaultState();
		m_FSM.AddState(m_DefaultState);

		m_ConcertState = new GameConcertState();
		m_FSM.AddState(m_ConcertState);

		m_GameOverState = new GameGameOverState();
		m_FSM.AddState(m_GameOverState);
	}
}
