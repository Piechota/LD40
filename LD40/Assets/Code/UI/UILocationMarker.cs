using UnityEngine;
using UnityEngine.UI;

public class UILocationMarker : AUIMarker
{
	[SerializeField]
	private Image m_TimerImage;

	public void UpdateTimer(float val)
	{
		m_TimerImage.fillAmount = val;
	}
}
