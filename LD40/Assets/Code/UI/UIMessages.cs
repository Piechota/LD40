using System.Collections.Generic;
using UnityEngine;

public class UIMessages : MonoBehaviour
{
	[SerializeField]
	private List<UIMessage> m_Messages = new List<UIMessage>();

	private List<string> m_Texts = new List<string>();

	private List<string> m_SMS = new List<string>()
	{
		"Good job, kiddo. Your concert was amazing! Next gig in {0} sec, good luck!",
		"You are the rock star! People love you! We’re gonna send your check after next song. We’re waiting!",
		"OMG! Did you see that crowd?! You are the hottest guy in the whole city! See ya in {0} sec!",
		"HOT! People are screaming your name EVERYWHERE! Btw, your next concert is starting in {0} sec. Hurry up!",
		"Don’t forget to sign my T-shirt I’m gonna make thousands on the i-bay. Anyway! Next show is in {0} sec!",
		"What a show! The merch is selling like hot cakes! Let’s hope the sales keep up at your next show!",
		"Your next show in location is in {0} sec. There are fangirls waiting backstage!",
		"Sorry, I lied about the fangirls. Anyway, there’s another show to go - next location, in {0} sec. Do your best!",
	};

	private string m_FailMessage = "That's IT, I won't tolerate any of this special-snowflake pop idol nonsense. You're FIRED! Go back to the country or something.";

	private void Awake()
	{
		m_Texts.Add(m_Messages[0].GetText());
		m_Texts.Add(m_Messages[1].GetText());
		m_Texts.Add(m_Messages[2].GetText());
	}

	public void ShowRandomMessage()
	{
		int rnd = Random.Range(0, m_SMS.Count);
		int timer = Mathf.RoundToInt(POIManager.Instance.MissionTimer);
		string text = string.Format(m_SMS[rnd], timer);
		ShowMessage(text);
	}

	public void ShowFailMessage()
	{
		ShowMessage(m_FailMessage);
	}

	private void ShowMessage(string message)
	{
		m_Messages[2].ShowText(m_Texts[1]);
		m_Texts[2] = m_Texts[1];

		m_Messages[1].ShowText(m_Texts[0]);
		m_Texts[1] = m_Texts[0];

		m_Messages[0].ShowText(message);
		m_Texts[0] = message;
	}
}
