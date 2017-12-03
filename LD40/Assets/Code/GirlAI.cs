using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GirlAI : CachedMonoBehaviour
{
	public readonly AEvent OnPairStarted = new AEvent();
	public readonly AEvent OnPairStopped = new AEvent();

	[SerializeField]
	private EventCollider m_PickupTrigger;
	public EventCollider PickupTrigger { get { return m_PickupTrigger; } }

	[SerializeField]
	private Material m_ConeMaterial;
	[SerializeField]
	private Color m_ConeMaterialIdle = Color.magenta;
	[SerializeField]
	private Color m_ConeMaterialSpotted = Color.red;

	public bool IsInitialized { get; private set; }

	public float SpotValue { get; private set; }
	public bool HasSpotted { get { return m_FSM.CurrentStateId == (int)EFanStateID.Spotted; } }

	private FiniteStateMachine m_FSM;
	public FiniteStateMachine FSM { get { return m_FSM; } }
	private FanIdleState m_IdleState;
	private FanSpottedState m_SpottedState;

	private NavMeshAgent m_Agent;
	public NavMeshAgent Agent { get { return m_Agent; } }

	[Header("Following")]
	[SerializeField]
	private float m_FollowingDistance;

    private SpawnPoint m_OriginPoint;

	private bool m_PlayerDetected = false;
	private bool m_ShowCone = false;

	private const string TINT_COLOR_PROPERTY = "_TintColor";

	private void Awake()
	{
		m_Agent = GetComponent<NavMeshAgent>();
		PrepareStateMachine();
	}

	private void Update()
	{
		m_FSM.Update();
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

	public void Initialize(SpawnPoint spawnPoint)
    {
        m_OriginPoint = spawnPoint;
        m_OriginPoint.IsUsed = true;
        gameObject.SetActive(true);
        IsInitialized = true;

		m_FSM.TransitionTo(m_IdleState.Id);
		m_IdleState.OnPlayerSpotted.AddListener(HandlePlayerSpotted);
		UIManager.Instance.CreateMarker(this);
	}

	public void Uninitialize()
	{
		IsInitialized = false;
		m_FSM.Reset();
		m_IdleState.OnPlayerSpotted.RemoveListener(HandlePlayerSpotted);
	}

	private void PrepareStateMachine()
	{
		m_FSM = new FiniteStateMachine();

		m_IdleState = new FanIdleState(this);
		m_FSM.AddState(m_IdleState);

		m_SpottedState = new FanSpottedState(this);
		m_FSM.AddState(m_SpottedState);
	}

	public bool DetectPlayer()
	{
		m_PlayerDetected = GirlsManager.Instance.FieldOfView.TestCollision(CachedTransform.position, CachedTransform.forward);
		m_ShowCone = true;

		return m_PlayerDetected;
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

	public void FollowPlayer()
	{
		PlayerController player = GameManager.Instance.Player;
		Vector3 playerPosition = player.CachedTransform.position;
		m_Agent.SetDestination(playerPosition);
	}

	public void SetTargetForward(Vector3 forward)
	{
		CachedTransform.forward = forward;
	}

	public void StartFollowing()
	{
		m_Agent.isStopped = false;
		m_OriginPoint.IsUsed = false;
		m_ShowCone = false;
	}

	public void StopFollowing()
	{
		m_Agent.isStopped = true;
		m_ShowCone = true;
	}

	public void Release()
	{
		GirlsManager.Instance.AddGirlToPool(this);
		m_OriginPoint.IsUsed = false; //just in case
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
		m_ConeMaterial.SetPass(0);
		m_ConeMaterial.SetColor(TINT_COLOR_PROPERTY, color);
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
