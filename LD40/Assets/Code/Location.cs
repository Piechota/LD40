using UnityEngine;

public class Location : MonoBehaviour
{
	[SerializeField]
	private EventCollider m_Collider;

	private void Awake()
	{
		m_Collider.OnTriggerEntered.AddListener(HandleCollision);
	}

	private void Update()
	{

	}

	private void OnDestroy()
	{
		m_Collider.OnTriggerEntered.RemoveListener(HandleCollision);
	}

	private void HandleCollision(Collider col)
	{
		if (col.gameObject.layer == PlayerController.LAYER)
		{
			PlayerController player = GameManager.Instance.Player;
			if (player.IsEscorting)
			{
				player.PerformDate(this);
			}
		}
	}
}
