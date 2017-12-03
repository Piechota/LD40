using UnityEngine;

public class FanShoutState : AFanState
{
    private float ShoutTime = 0f;
    public FanShoutState(GirlAI fan) : base(EFanStateID.Shout, fan)
	{
    }

    protected override void HandleEnter(AState prevState)
    {
        ShoutTime = m_Fan.Params.ShoutTime;
        Shout();
    }

    protected override void HandleUpdate()
    {
        base.HandleUpdate();
        ShoutTime -= GameManager.Instance.DeltaTime;
        if ( ShoutTime < 0f)
        {
            m_Fan.SetSpottedState();
        }
    }

    protected override void HandleLeave(AState nextState)
    {
    }

    private void Shout()
    {
        Collider[] girls = Physics.OverlapSphere(m_Fan.CachedTransform.position, m_Fan.Params.ShoutRadius, 1 << 9);
        int girlsNum = girls.Length;
        for ( int i = 0; i < girlsNum; ++i )
        {
            GirlAI girlAI = girls[i].GetComponent<GirlAI>();
            if ( girlAI != m_Fan)
            {
                girlAI.Achtung(GameManager.Instance.Player.CachedTransform.position);
            }
        }
    }
}
