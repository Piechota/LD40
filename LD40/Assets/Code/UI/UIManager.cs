using UnityEngine;

public class UIManager : ASingleton<UIManager>
{
	[SerializeField]
	private CanvasGroup m_GameOverGroup;

	[SerializeField]
	private UIPartnerTimer m_PartnerTimerPrefab;

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

	public void CreatePartnerTimer(GirlAI girl)
	{
		UIPartnerTimer timer = Instantiate(m_PartnerTimerPrefab, CachedTransform);
		timer.Initialize(girl);
	}

	public void ShowGameOverPanel()
	{
		m_GameOverGroup.alpha = 1;
	}
}
