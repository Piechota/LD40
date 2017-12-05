using TMPro;
using UnityEngine;

public class UIMessage : CachedUIBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_Text;

	public void ShowText(string text)
	{
		m_Text.text = text;
	}

	public string GetText()
	{
		return m_Text.text;
	}
}
