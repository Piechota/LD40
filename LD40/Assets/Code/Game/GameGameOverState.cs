using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGameOverState : AGameState
{
	public GameGameOverState() : base(EGameState.GameOver)
	{
	}

	protected override void HandleEnter(AState prevState)
	{
		m_Mgr.SetGameTimerPause(true);
		m_Mgr.Player.SetInputLock(true);
	}

	protected override void HandleUpdate()
	{
		base.HandleUpdate();

		if (Input.anyKey)
		{
			SceneManager.LoadScene(0);
		}
	}

	protected override void HandleLeave(AState nextState)
	{
	}
}