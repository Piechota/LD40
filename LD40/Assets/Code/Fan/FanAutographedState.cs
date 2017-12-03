using UnityEngine;

public class FanAutographedState : AFanState
{
    private float m_RotationTimer = 0f;
    private Vector3 m_TargetForward;
    private float m_Time = 0f;

    public FanAutographedState(GirlAI partner) : base(EFanStateID.Autographed, partner)
	{
    }

    protected override void HandleEnter(AState prevState)
    {
        m_Fan.SetConeActive(false);
        m_Time = Random.Range(m_Fan.Params.AutographedTime.x, m_Fan.Params.AutographedTime.y);
    }

    protected override void HandleUpdate()
    {
        base.HandleUpdate();

        UpdateTargetRotation();

        m_Time -= GameManager.Instance.DeltaTime;
        if (m_Time < 0f)
        {
            m_Fan.SetRoamingState();
        }

    }

    private void UpdateTargetRotation()
    {
        m_RotationTimer -= GameManager.Instance.DeltaTime;
        if (m_RotationTimer <= 0)
        {
            m_RotationTimer = Random.Range(m_Fan.Params.AutographedRotationDelay.x, m_Fan.Params.AutographedRotationDelay.y);

            Vector2 circleForward = Random.insideUnitCircle;
            m_TargetForward = new Vector3(circleForward.x, 0, circleForward.y).normalized;
        }

        float lerp = GameManager.Instance.DeltaTime * m_Fan.Params.AutographedRotationSpeed;
        Vector3 forward = Vector3.Lerp(m_Fan.CachedTransform.forward, m_TargetForward, lerp);
        m_Fan.SetTargetForward(forward);
    }

    protected override void HandleLeave(AState nextState)
    {
        m_Fan.SetConeActive(true);
    }
}
