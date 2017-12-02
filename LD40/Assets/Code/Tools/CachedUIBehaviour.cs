using UnityEngine;
using UnityEngine.EventSystems;

public class CachedUIBehaviour : UIBehaviour
{
	private RectTransform m_CachedTransform;
	public RectTransform CachedTransform
	{
		get
		{
			if (m_CachedTransform == null)
			{
				m_CachedTransform = transform as RectTransform;
			}
			return m_CachedTransform;
		}
	}
}