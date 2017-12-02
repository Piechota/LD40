using UnityEngine;

public abstract class ASingleton<T> : CachedMonoBehaviour
	where T : MonoBehaviour
{
	private static T m_Instance;
	public static T Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = FindObjectOfType<T>();
			}
			return m_Instance;
		}
	}
}
