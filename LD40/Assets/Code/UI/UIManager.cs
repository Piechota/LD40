using UnityEngine;

public class UIManager : ASingleton<UIManager>
{
	[SerializeField]
	private CanvasGroup m_GameOverGroup;

	[SerializeField]
	private UIFanMarker m_FanMarkerPrefab;

	private void Awake()
	{
		GameManager.Instance.OnGameOver.AddListener(ShowGameOverPanel);
	}

	private void OnDestroy()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnGameOver.RemoveListener(ShowGameOverPanel);
		}
	}

	public void CreateMarker(GirlAI fan)
	{
		UIFanMarker marker = Instantiate(m_FanMarkerPrefab, CachedTransform);
		marker.Initialize(fan);
	}

	public void ShowGameOverPanel()
	{
		m_GameOverGroup.alpha = 1;
	}
}
