using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GirlAI : CachedMonoBehaviour
{
	public readonly AEvent OnFollowStarted = new AEvent();
	public readonly AEvent OnFollowStopped = new AEvent();

	[SerializeField]
	private EventCollider m_PickupTrigger;

	[SerializeField]
	public Material ConeMaterial;

	private float m_WaitTimer = 0f;
	private float m_WaitDuration = 30f;
	public float TimerValue { get { return Mathf.Clamp01(m_WaitTimer / m_WaitDuration); } }

	public bool IsInitialized { get; private set; }

	private bool m_PlayerDetected;
	public bool IsFollowing { get; private set; }

	private NavMeshAgent m_Agent;
	[Header("Following")]
	[SerializeField]
	private float m_FollowingDistance;

    public SpawnPoint OriginPoint;

	private void Awake()
	{
		m_Agent = GetComponent<NavMeshAgent>();
		m_PickupTrigger.OnTriggerEntered.AddListener(HandlePickupTriggerEntered);
		m_PickupTrigger.OnTriggerExited.AddListener(HandlePickupTriggerExited);
		IsFollowing = false;
	}

	private void Update()
	{
		if (IsInitialized)
		{
			if (!IsFollowing)
			{
				m_WaitTimer -= Time.deltaTime;
				m_PlayerDetected = GirlsManager.Instance.FieldOfView.TestCollision(CachedTransform.position, CachedTransform.forward);
			}
			else
			{
				FollowPlayer();
			}
		}
	}

	private void OnDestroy()
	{
		m_PickupTrigger.OnTriggerEntered.RemoveListener(HandlePickupTriggerEntered);
		m_PickupTrigger.OnTriggerExited.RemoveListener(HandlePickupTriggerExited);
	}

	public void Initialize(SpawnPoint origin)
	{
		IsInitialized = true;
		m_WaitTimer = m_WaitDuration;
		OriginPoint = origin;
	}

	public void Uninitialize()
	{
		IsInitialized = false;
		StopFollowing();
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

	private void FollowPlayer()
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

	void OnRenderObject()
	{
		DrawCone(m_PlayerDetected ? Color.red : Color.blue);
	}

	public void ToggleFollowing()
	{
		if (!IsInitialized)
		{
			return;
		}

		if (!IsFollowing)
		{
			StartFollowing();
		}
		else
		{
			StopFollowing();
		}
	}

	public void StartFollowing()
	{
		IsFollowing = true;
		m_Agent.isStopped = false;
		OnFollowStarted.Invoke();
        OriginPoint.IsUsed = false;
	}

	public void StopFollowing()
	{
		IsFollowing = false;
		m_Agent.isStopped = true;
	}

    public void DateFinished()
    {
        GirlsManager.Instance.AddGirlToPool(this);
        OriginPoint.IsUsed = false; //just in case
    }

    private void HandlePickupTriggerEntered(Collider col)
	{
		if (col.gameObject.layer == PlayerController.LAYER)
		{
			GameManager.Instance.Player.AddPickupOption(this);
		}
	}

	private void HandlePickupTriggerExited(Collider col)
	{
		if (col.gameObject.layer == PlayerController.LAYER)
		{
			GameManager.Instance.Player.RemovePickupOption(this);
		}
	}
}
