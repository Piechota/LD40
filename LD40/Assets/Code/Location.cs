using UnityEngine;

public enum ELocationType
{
	Back,
	Angle
}

public class Location : CachedMonoBehaviour
{
	[SerializeField]
	private Transform m_MarkerSpawnPoint;
	public Transform MarkerSpawnPoint { get { return m_MarkerSpawnPoint; } }
	[SerializeField]
	private ELocationType m_Type;
	public ELocationType Type { get { return m_Type; } }

	[SerializeField]
	private EventCollider m_Collider;

    public int ID { get; set; }

	private void Awake()
	{
		m_Collider.OnTriggerEntered.AddListener(HandleCollision);
		SetTarget(false);
		POIManager.Instance.RegisterLocation(this);
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		m_Collider.OnTriggerEntered.RemoveListener(HandleCollision);
	}

	public void SetTarget(bool set)
	{
		m_Collider.gameObject.SetActive(set);
	}

	private void HandleCollision(Collider col)
	{
		if (col.gameObject.layer == PlayerController.LAYER)
		{
			if (POIManager.Instance.TargetLocation == this)
			{
				//PlayerController player = GameManager.Instance.Player;
				POIManager.Instance.CompleteActiveMission();
			}
		}
	}
}
