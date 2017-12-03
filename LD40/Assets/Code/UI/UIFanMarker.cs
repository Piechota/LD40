using UnityEngine;
using UnityEngine.UI;

public class UIFanMarker : CachedUIBehaviour
{
	[SerializeField]
	private CanvasGroup m_CanvasGroup;
	[SerializeField]
	private Image m_Image;

	public bool IsInitialized { get; private set; }
	private Camera m_MainCamera;
	private GirlAI m_Fan;

	private readonly static Vector3 OFFSET = new Vector3(0, 2, 0);

	private void Update()
	{
		if (IsInitialized)
		{
			UpdateTimer();
		}
	}

	public void Initialize(GirlAI fan)
	{
		IsInitialized = true;
		m_MainCamera = Camera.main;

		m_Fan = fan;
	}

	public void Uninitialize()
	{
		m_Fan = null;
		IsInitialized = false;
	}

	private void UpdateTimer()
	{
		Vector3 pivotPoint = m_Fan.CachedTransform.position + OFFSET;
		Vector3 screenPoint = m_MainCamera.WorldToScreenPoint(pivotPoint);

		float halfWidth = CachedTransform.rect.width / 2f;
		screenPoint.x = Mathf.Clamp(screenPoint.x, halfWidth, Screen.width - halfWidth);
		float halfHeight = CachedTransform.rect.height / 2f;
		screenPoint.y = Mathf.Clamp(screenPoint.y, halfHeight, Screen.height - halfHeight);

		transform.position = screenPoint;
	}
}
