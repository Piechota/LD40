using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GirlAI : CachedMonoBehaviour
{
	public readonly AEvent OnPairStarted = new AEvent();
	public readonly AEvent OnPairStopped = new AEvent();

	[SerializeField]
	private FanParams m_Params;
	public FanParams Params { get { return m_Params; } }
	[SerializeField]
	private ParticleSystem m_HeartParticles;
	public ParticleSystem HeartParticles { get { return m_HeartParticles; } }

    [SerializeField]
    public AudioClip[] m_Squeaking;
    [HideInInspector]
    public AudioSource m_AudioSource;

    [SerializeField]
	private Material m_ConeMaterial;
	private Material m_ConeMaterialInstance;
	[SerializeField]
	private Color m_ConeMaterialIdle = Color.magenta;
	[SerializeField]
	private Color m_ConeMaterialSpotted = Color.red;

	public bool IsInitialized { get; private set; }

	public float SpotValue { get; private set; }
	public bool HasSpotted { get { return m_FSM.CurrentStateId == (int)EFanStateID.Spotted; } }
    public bool Blind { get; set; }

	private FiniteStateMachine m_FSM;
	public FiniteStateMachine FSM { get { return m_FSM; } }
	private FanIdleState m_IdleState;
	private FanRoamingState m_RoamingState;
	private FanSpottedState m_SpottedState;
    private FanShoutState m_ShoutState;
    private FanAutographedState m_AutographedState;

    private NavMeshAgent m_Agent;
	public NavMeshAgent Agent { get { return m_Agent; } }

	private bool m_PlayerDetected = false;
	private bool m_ShowCone = false;

	private const float SPOT_SPEED = 4f;
	private const string TINT_COLOR_PROPERTY = "_TintColor";

	private void Awake()
	{
		m_HeartParticles.Stop();
		m_ConeMaterialInstance = new Material(m_ConeMaterial);
		m_Agent = GetComponent<NavMeshAgent>();
        m_AudioSource = GetComponent<AudioSource>();

        PrepareStateMachine();
        Blind = false;
    }

	private void Update()
	{
		m_FSM.Update();
	}

	private void FixedUpdate()
	{
		m_FSM.FixedUpdate();
	}

	private void OnRenderObject()
	{
		if (m_ShowCone)
		{
			Color spotColor = Color.Lerp(m_ConeMaterialIdle, m_ConeMaterialSpotted, SpotValue);
			DrawCone(spotColor);
		}
	}

	private void OnDestroy()
	{
	}

	public void Initialize()
    {
        gameObject.SetActive(true);
        IsInitialized = true;
		UIManager.Instance.CreateFanMarker(this);
		m_FSM.TransitionTo(m_IdleState);
	}

	public void Uninitialize()
	{
		IsInitialized = false;
		m_FSM.Reset();
	}

	private void PrepareStateMachine()
	{
		m_FSM = new FiniteStateMachine();

		m_IdleState = new FanIdleState(this);
		m_FSM.AddState(m_IdleState);

		m_RoamingState = new FanRoamingState(this);
		m_FSM.AddState(m_RoamingState);

		m_SpottedState = new FanSpottedState(this);
		m_FSM.AddState(m_SpottedState);

        m_ShoutState = new FanShoutState(this);
        m_FSM.AddState(m_ShoutState);

        m_AutographedState = new FanAutographedState(this);
        m_FSM.AddState(m_AutographedState);
    }

    public void DetectPlayer()
	{
		m_PlayerDetected = !Blind && GirlsManager.Instance.FieldOfView.TestCollision(CachedTransform.position, CachedTransform.forward);
		m_ShowCone = !Blind;

		float spotValue = GameManager.Instance.DeltaTime * SPOT_SPEED;
		if (m_PlayerDetected)
		{
			UpdateSpotValue(spotValue);
			if (SpotValue >= 1)
			{
				ResetSpotValue();
				m_FSM.TransitionTo(m_ShoutState);
			}
		}
		else
		{
			UpdateSpotValue(-spotValue);
		}
	}

	public void UpdateSpotValue(float add)
	{
		SpotValue += add;
		SpotValue = Mathf.Clamp01(SpotValue);
	}

	public void ResetSpotValue()
	{
		SpotValue = 0;
	}

	public void SetTargetDestination(Vector3 position)
	{
		m_Agent.SetDestination(position);
	}

	public void SetTargetForward(Vector3 forward)
	{
		CachedTransform.forward = forward;
	}

	public void UnlockNavigation()
	{
		m_Agent.isStopped = false;
	}

	public void LockNavigation()
	{
		m_Agent.isStopped = true;
	}

	public void SetConeActive(bool set)
	{
		m_ShowCone = set;
	}

	public void SetAgentSpeed(float speed)
	{
		m_Agent.speed = speed;
	}

    public void SetIdleState()
    {
        m_FSM.TransitionTo(m_IdleState);
    }
    public void SetRoamingState()
    {
        m_FSM.TransitionTo(m_RoamingState);
    }
    public void SetSpottedState()
    {
        m_FSM.TransitionTo(m_SpottedState);
    }

    public void Achtung(Vector3 position)
    {
        if ( m_FSM.CurrentStateId == (int)EFanStateID.Idle)
        {
            SetRoamingState();
        }
        if (m_FSM.CurrentStateId == (int)EFanStateID.Roaming)
        {
            SetTargetDestination(position);
        }
    }
    public void GetAutographed()
    {
        m_FSM.TransitionTo(m_AutographedState);
    }

    public float GetDistance()
    {
        return m_Agent.remainingDistance;
    }

    private void HandlePlayerSpotted()
	{
		m_FSM.TransitionTo(m_SpottedState);
	}

	private void DrawCone(Color color)
	{
		GirlFOV fieldOfView = GirlsManager.Instance.FieldOfView;
		const int triangleNum = 20;

		float deltaDegree = 2f * fieldOfView.ConeDegree * Mathf.Deg2Rad / triangleNum;
		float cosDelta = Mathf.Cos(deltaDegree);
		float sinDelta = Mathf.Sin(deltaDegree);

		float coneCos = Mathf.Cos(-fieldOfView.ConeDegree * Mathf.Deg2Rad);
		float coneSin = Mathf.Sin(-fieldOfView.ConeDegree * Mathf.Deg2Rad);

		Vector3 initDir = new Vector3(coneSin * CachedTransform.forward.z + coneCos * CachedTransform.forward.x, 0f, coneCos * CachedTransform.forward.z - coneSin * CachedTransform.forward.x);
		initDir.Normalize();
		Vector3 pos0 = CachedTransform.position;
		float coneRadius = fieldOfView.RaysDistance;
		GL.Begin(GL.TRIANGLES);
		m_ConeMaterialInstance.SetPass(0);
		m_ConeMaterialInstance.SetColor(TINT_COLOR_PROPERTY, color);
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
}
