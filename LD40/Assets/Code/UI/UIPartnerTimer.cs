using UnityEngine;
using UnityEngine.UI;

public class UIPartnerTimer : CachedUIBehaviour
{
	[SerializeField]
	private CanvasGroup m_CanvasGroup;
	[SerializeField]
	private Image m_Image;

	public bool IsInitialized { get; private set; }
	private Camera m_MainCamera;
	private GirlAI m_Partner;

	private readonly static Vector3 TIMER_OFFSET = new Vector3(0, 3, 0);

	private void LateUpdate()
	{
		if (IsInitialized)
		{
			UpdateTimer();
		}
	}

	public void Initialize(GirlAI partner)
	{
		IsInitialized = true;
		m_MainCamera = Camera.main;
		HandleFollowStopped();

		m_Partner = partner;
		m_Partner.OnPairStarted.AddListener(HandleFollowStarted);
		m_Partner.OnPairStopped.AddListener(HandleFollowStopped);
	}

	public void Uninitialize()
	{
		m_Partner.OnPairStarted.RemoveListener(HandleFollowStarted);
		m_Partner.OnPairStopped.RemoveListener(HandleFollowStopped);

		m_Partner = null;
		IsInitialized = false;
	}

	private void UpdateTimer()
	{
		Vector3 pivotPoint = m_Partner.CachedTransform.position + TIMER_OFFSET;
		Vector3 screenPoint = m_MainCamera.WorldToScreenPoint(pivotPoint);

		float halfWidth = CachedTransform.rect.width / 2f;
		screenPoint.x = Mathf.Clamp(screenPoint.x, halfWidth, Screen.width - halfWidth);
		float halfHeight = CachedTransform.rect.height / 2f;
		screenPoint.y = Mathf.Clamp(screenPoint.y, halfHeight, Screen.height - halfHeight);

		transform.position = screenPoint;

		m_Image.fillAmount = m_Partner.TimerValue;
	}

	private void HandleFollowStarted()
	{
		m_CanvasGroup.alpha = 0f;
	}

	private void HandleFollowStopped()
	{
		m_CanvasGroup.alpha = 1f;
	}
}
