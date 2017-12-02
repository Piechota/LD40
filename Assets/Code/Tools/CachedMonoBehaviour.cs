using UnityEngine;

public class CachedMonoBehaviour : MonoBehaviour
{
	private Transform m_CachedTransform;
	public Transform CachedTransform
	{
		get
		{
			if (m_CachedTransform == null)
			{
				m_CachedTransform = transform;
			}
			return m_CachedTransform;
		}
	}
}