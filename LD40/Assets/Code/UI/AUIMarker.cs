﻿using UnityEngine;

public abstract class AUIMarker : CachedUIBehaviour
{
	public bool IsInitialized { get; private set; }
	private Transform m_Target;
	private Camera m_MainCamera;

	protected Vector3 m_ScreenPoint = new Vector3();
	protected Vector3 m_ClampedScreenPoint = new Vector3();

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
		m_ScreenPoint = m_MainCamera.WorldToScreenPoint(pivotPoint);
		float halfWidth = CachedTransform.rect.width / 2f;
		float halfHeight = CachedTransform.rect.height / 2f;

		if (m_ScreenPoint.z < 0)
		{
			m_ScreenPoint *= -1;
		}

		m_ClampedScreenPoint.x = Mathf.Clamp(m_ScreenPoint.x, m_MainCamera.pixelRect.x + halfWidth, m_MainCamera.pixelRect.width - halfWidth + m_MainCamera.pixelRect.x);
		m_ClampedScreenPoint.y = Mathf.Clamp(m_ScreenPoint.y, m_MainCamera.pixelRect.y + halfHeight, m_MainCamera.pixelRect.height - halfHeight + m_MainCamera.pixelRect.y);
		transform.position = m_ClampedScreenPoint;
	}
}
