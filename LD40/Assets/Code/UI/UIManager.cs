using TMPro;
using UnityEngine;

public class UIManager : ASingleton<UIManager>
{
	[SerializeField]
	private UIMessages m_Messages;
	public UIMessages Messages { get { return m_Messages; } }

	[SerializeField]
	private CanvasGroup m_GameOverGroup;
	[SerializeField]
	private TextMeshProUGUI m_LastedText;

	[SerializeField]
	private UILocationMarker m_LocationMarker;
	[SerializeField]
	private UIFanMarker m_FanMarkerPrefab;

	[SerializeField]
	private TextMeshProUGUI m_TimerLabel;
	[SerializeField]
	private TextMeshProUGUI m_MissionCounter;
	[SerializeField]
	private UICoolDown m_AutographCooldown;

	private void Awake()
	{
		GameManager.Instance.OnGameOver.AddListener(ShowGameOverPanel);
	}

	private void Update()
	{
		UpdateMissionTimer();
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
            m_AutographCooldown.UpdateCoolDown(1 - val);
        }
    }

	public void UpdateMissionTimer()
	{
		int timer = Mathf.RoundToInt(POIManager.Instance.MissionTimer);
		int mins = Mathf.FloorToInt(timer / 60);
		int secs = timer % 60;
		string secDigit = "";
		if (secs < 10)
		{
			secDigit = "0";
		}

		m_TimerLabel.color = (timer < 10) ? new Color(1f, 0.5f, 0.5f) : Color.white;
		m_TimerLabel.text = string.Format("0{0}:{1}{2}", mins, secDigit, secs);
	}

	public void UpdateMissionCounter(int count)
	{
		m_MissionCounter.text = string.Format("<b>{0}</b> concerts given", count);
	}

	public void ShowGameOverPanel()
	{
		m_GameOverGroup.alpha = 1;
		m_LastedText.text = string.Format("You gave <b>{0}</b> concerts before your short career was over and newer, younger artists took your place.", POIManager.Instance.MissionCounter);
	}
}
