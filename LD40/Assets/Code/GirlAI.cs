using System.Collections.Generic;
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
	public Material ConeMaterial;

	private float m_WaitTimer = 0f;
	private float m_WaitDuration = 10f;
	public float TimerValue { get { return Mathf.Clamp01(m_WaitTimer / m_WaitDuration); } }

	public bool IsInitialized { get; private set; }

	private bool m_PlayerDetected;
	public bool IsPaired { get { return m_FSM.CurrentStateId == (int)EPartnerStateID.Pair; } }
	public bool IsAngry { get { return m_FSM.CurrentStateId == (int)EPartnerStateID.Angry; } }

	private FiniteStateMachine m_FSM;
	public FiniteStateMachine FSM { get { return m_FSM; } }
	private PartnerIdleState m_IdleState;
	private PartnerPairState m_FollowState;
	private PartnerAngryState m_AngryState;

	private NavMeshAgent m_Agent;
	public NavMeshAgent Agent { get { return m_Agent; } }

	[Header("Following")]
	[SerializeField]
	private float m_FollowingDistance;

    private SpawnPoint m_OriginPoint;
    private List<SpawnPoint> m_DestinationPoint; 

	private void Awake()
	{
		m_Agent = GetComponent<NavMeshAgent>();
		PrepareStateMachine();
	}

	private void Update()
	{
		m_FSM.Update();
	}

	private void OnDestroy()
	{
	}

	public void Initialize(SpawnPoint spawnPoint, List<SpawnPoint> destinationPoints)
    {
        m_OriginPoint = spawnPoint;
        m_DestinationPoint = destinationPoints;
        m_OriginPoint.IsUsed = true;
        gameObject.SetActive(true);
        IsInitialized = true;
		m_WaitTimer = m_WaitDuration;

		m_FSM.TransitionTo(m_IdleState.Id);
		m_IdleState.OnWaitFinished.AddListener(HandleIdleWaitFinished);
		UIManager.Instance.CreatePartnerTimer(this);
    }

    public void Uninitialize()
	{
		IsInitialized = false;
		StopPair();
		m_FSM.Reset();
		m_IdleState.OnWaitFinished.RemoveListener(HandleIdleWaitFinished);
	}

	private void PrepareStateMachine()
	{
		m_FSM = new FiniteStateMachine();

		m_IdleState = new PartnerIdleState(this);
		m_FSM.AddState(m_IdleState);

		m_FollowState = new PartnerPairState(this);
		m_FSM.AddState(m_FollowState);

		m_AngryState = new PartnerAngryState(this);
		m_FSM.AddState(m_AngryState);
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
		ConeMaterial.SetPass(0);
		ConeMaterial.color = color;
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

	private void OnRenderObject()
	{
		if (m_FSM.CurrentStateId != (int)EPartnerStateID.Pair)
		{
			m_PlayerDetected = GirlsManager.Instance.FieldOfView.TestCollision(CachedTransform.position, CachedTransform.forward);
			DrawCone(m_PlayerDetected ? Color.red : Color.blue);
		}
	}

	public void FollowPlayer()
	{
		PlayerController player = GameManager.Instance.Player;
		Vector3 playerPosition = player.CachedTransform.position;
		Vector3 positionRight = playerPosition + player.CachedTransform.right * m_FollowingDistance;
		Vector3 positionLeft = playerPosition - player.CachedTransform.right * m_FollowingDistance;

		if (Vector3.SqrMagnitude(CachedTransform.position - positionRight) < Vector3.SqrMagnitude(CachedTransform.position - positionLeft))
		{
			m_Agent.SetDestination(positionRight);
		}
		else
		{
			m_Agent.SetDestination(positionLeft);
		}
	}

	public void StartPair()
	{
		m_FSM.TransitionTo(m_FollowState);
		OnPairStarted.Invoke();
	}

	public void StopPair()
	{
		if (m_WaitTimer > 0)
		{
			m_FSM.TransitionTo(m_IdleState);
		}
		else
		{
			m_FSM.TransitionTo(m_AngryState);
		}

		OnPairStopped.Invoke();
	}

	public void StartFollowing()
	{
		m_Agent.isStopped = false;
		m_OriginPoint.IsUsed = false;
	}

	public void StopFollowing()
	{
		m_Agent.isStopped = true;
	}

	public void UpdateWaitTimer()
	{
		m_WaitTimer -= Time.deltaTime;
    }

    public void DateFinished()
    {
        GirlsManager.Instance.AddGirlToPool(this);
        m_OriginPoint.IsUsed = false; //just in case
    }

	private void HandleIdleWaitFinished()
	{
		m_FSM.TransitionTo(m_AngryState);
	}
}
