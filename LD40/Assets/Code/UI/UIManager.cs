using UnityEngine;

public class UIManager : ASingleton<UIManager>
{
	[SerializeField]
	private UIPartnerTimer m_PartnerTimerPrefab;

	public void CreatePartnerTimer(GirlAI girl)
	{
		UIPartnerTimer timer = Instantiate(m_PartnerTimerPrefab, CachedTransform);
		timer.Initialize(girl);
	}
}
