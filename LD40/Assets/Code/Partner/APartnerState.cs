public abstract class APartnerState : AState
{
	public EPartnerStateID StateId { get; private set; }
	protected GirlAI m_Partner;

	public APartnerState(EPartnerStateID id, GirlAI partner) : base((int)id)
	{
		StateId = id;
		m_Partner = partner;
	}
}
