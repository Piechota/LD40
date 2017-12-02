using UnityEngine;

public class EventCollider : MonoBehaviour
{
	public Collider Collider;

	public AEvent<Collision> OnColliderEntered = new AEvent<Collision>();
	public AEvent<Collision> OnColliderStayed = new AEvent<Collision>();
	public AEvent<Collision> OnColliderExited = new AEvent<Collision>();

	public AEvent<Collider> OnTriggerEntered = new AEvent<Collider>();
	public AEvent<Collider> OnTriggerStayed = new AEvent<Collider>();
	public AEvent<Collider> OnTriggerExited = new AEvent<Collider>();

	private void Awake()
	{
		Collider = GetComponent<Collider>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		OnColliderEntered.Invoke(collision);
	}

	private void OnCollisionStay(Collision collision)
	{
		OnColliderStayed.Invoke(collision);
	}

	private void OnCollisionExit(Collision collision)
	{
		OnColliderExited.Invoke(collision);
	}

	private void OnTriggerEnter(Collider other)
	{
		OnTriggerEntered.Invoke(other);
	}

	private void OnTriggerStay(Collider other)
	{
		OnTriggerStayed.Invoke(other);
	}

	private void OnTriggerExit(Collider other)
	{
		OnTriggerExited.Invoke(other);
	}
}
