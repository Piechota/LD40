using UnityEngine;

public class UIFanMarker : AUIMarker
{
	[SerializeField]
	private CanvasGroup m_Group;

	protected override void HandleUpdate()
	{
		if (m_ScreenPoint.x != m_ClampedScreenPoint.x && m_ScreenPoint.y != m_ClampedScreenPoint.y)
		{
			m_Group.alpha = 1f;
		}
		else
		{
			m_Group.alpha = 0f;
		}
	}
}
