public abstract class AFanState : AState
{
	public EFanStateID StateId { get; private set; }
	protected GirlAI m_Fan;

	public AFanState(EFanStateID id, GirlAI fan) : base((int)id)
	{
		StateId = id;
		m_Fan = fan;
	}
}
