using UnityEngine;
using UnityEngine.UI;

public class UICoolDown : CachedUIBehaviour
{
    [SerializeField]
    private Image m_TimerImage;

    public void UpdateCoolDown(float val)
    {
		m_TimerImage.fillAmount = val;
    }
}
