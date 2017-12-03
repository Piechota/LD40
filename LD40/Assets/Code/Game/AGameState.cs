public abstract class AGameState : AState
{
	protected GameManager m_Mgr { get { return GameManager.Instance; } }

	public AGameState(EGameState state) : base((int)state)
	{
	}
}
