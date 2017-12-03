using UnityEngine;

public abstract class AUIMarker : CachedUIBehaviour
{
	public bool IsInitialized { get; private set; }
	private Transform m_Target;
	private Camera m_MainCamera;

	protected virtual Vector3 VERTICAL_OFFSET { get { return new Vector3(0, 2, 0); }  }

	protected virtual void Update()
	{
		if (IsInitialized)
		{
			UpdatePositioning();
			HandleUpdate();
		}
	}

	public void Initialize(Transform target)
	{
		IsInitialized = true;
		m_Target = target;
		m_MainCamera = Camera.main;
	}

	public void Uninitialize()
	{
		m_Target = null;
		IsInitialized = false;
	}

	protected virtual void HandleUpdate()
	{
	}

	private void UpdatePositioning()
	{
		Vector3 pivotPoint = m_Target.position + VERTICAL_OFFSET;
		Vector3 screenPoint = m_MainCamera.WorldToScreenPoint(pivotPoint);
		float halfWidth = CachedTransform.rect.width / 2f;
		float halfHeight = CachedTransform.rect.height / 2f;

		if (screenPoint.z < 0)
		{
			screenPoint *= -1;
		}

		screenPoint.x = Mathf.Clamp(screenPoint.x, halfWidth, Screen.width - halfWidth);
		screenPoint.y = Mathf.Clamp(screenPoint.y, halfHeight, Screen.height - halfHeight);
		transform.position = screenPoint;
	}
}
