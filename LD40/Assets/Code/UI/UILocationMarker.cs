using UnityEngine;
using UnityEngine.UI;

public class UILocationMarker : AUIMarker
{
	[SerializeField]
	private Image m_TimerImage;

	protected override void HandleUpdate()
	{
		UpdateTimer(POIManager.Instance.MissionTimerValue);
	}

	public void UpdateTimer(float val)
	{
		m_TimerImage.fillAmount = val;
	}
}
