using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGameOverState : AGameState
{
    private float m_Time = 0f;
	public GameGameOverState() : base(EGameState.GameOver)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Mgr.SetGameTimerPause(true);
		m_Mgr.Player.SetInputLock(true);
        m_Time = 1f;
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();
        m_Time -= Time.deltaTime;
        Debug.Log(m_Time);
		if (m_Time < 0f && Input.anyKeyDown)
		{
			SceneManager.LoadScene(0);
		}
	}

	protected override void HandleLeave(AState nextState)
	{
	}
}