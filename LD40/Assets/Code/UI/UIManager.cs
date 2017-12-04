using UnityEngine;

public class UIManager : ASingleton<UIManager>
{
	[SerializeField]
	private CanvasGroup m_GameOverGroup;

	[SerializeField]
	private UILocationMarker m_LocationMarker;
	[SerializeField]
	private UIFanMarker m_FanMarkerPrefab;

	[SerializeField]
	private UICoolDown m_AutographCooldown;

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

	public void CreateFanMarker(GirlAI fan)
	{
		UIFanMarker marker = Instantiate(m_FanMarkerPrefab, CachedTransform);
		marker.Initialize(fan.CachedTransform);
	}

	public void ShowLocationMarker(Location loc)
	{
		if (loc != null)
		{
			m_LocationMarker.gameObject.SetActive(true);
			m_LocationMarker.Initialize(loc.MarkerSpawnPoint);
		}
		else
		{
			m_LocationMarker.gameObject.SetActive(false);
		}
	}

    public void UpdateUICoolDown(float val)
    {
        if (m_AutographCooldown)
        {
            m_AutographCooldown.UpdateCoolDown(val);
        }
    }

	public void ShowGameOverPanel()
	{
		m_GameOverGroup.alpha = 1;
	}
}
