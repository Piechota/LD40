using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class GirlAI : CachedMonoBehaviour
{
	[SerializeField]
	private EventCollider m_PickupTrigger;

	[SerializeField]
	public Material ConeMaterial;

	private bool m_Catched;
	public bool IsFollowing { get; set; }

	private NavMeshAgent m_Agent;
	[Header("Following")]
	[SerializeField]
	private float m_FollowingDistance;

    private SpawnPoint m_OriginPoint;
    private List<SpawnPoint> m_DestinationPoint; 
    private void Awake()
	{
		m_Agent = GetComponent<NavMeshAgent>();
		m_PickupTrigger.OnTriggerEntered.AddListener(HandlePickupTriggerEntered);
		m_PickupTrigger.OnTriggerExited.AddListener(HandlePickupTriggerExited);
		IsFollowing = false;
	}

	private void OnDestroy()
	{
		m_PickupTrigger.OnTriggerEntered.RemoveListener(HandlePickupTriggerEntered);
		m_PickupTrigger.OnTriggerExited.RemoveListener(HandlePickupTriggerExited);
	}

	private void DrawCone(Color color)
	{
		FieldOfView fieldOfView = GirlsManager.Instance.FieldOfView;
		const int triangleNum = 20;

		float deltaDegree = 2f * fieldOfView.ConeDegree * Mathf.Deg2Rad / (float)triangleNum;
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
		DrawCone(m_Catched ? Color.red : Color.blue);
	}
	// Update is called once per frame
	void Update()
	{
		if (!IsFollowing)
		{
			m_Catched = GirlsManager.Instance.FieldOfView.TestCollision(CachedTransform.position, CachedTransform.forward);
		}
		else
		{
			FollowPlayer();
		}
	}

	public void ToggleFollowing()
	{
		if (!IsFollowing)
		{
			StartFollowing();
		}
		else
		{
			StopFollowing();
		}
	}

	public void StopFollowing()
	{
		IsFollowing = false;
		m_Agent.isStopped = true;
	}

	public void StartFollowing()
	{
		IsFollowing = true;
		m_Agent.isStopped = false;
        m_OriginPoint.IsUsed = false;
    }

    public void DateFinished()
    {
        GirlsManager.Instance.AddGirlToPool(this);
        m_OriginPoint.IsUsed = false; //just in case
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

    public void Spawn( SpawnPoint spawnPoint, List<SpawnPoint> destinationPoints )
    {
        m_OriginPoint = spawnPoint;
        m_DestinationPoint = destinationPoints;
        gameObject.SetActive( true );
    }
}
