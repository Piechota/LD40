using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerLocomotionController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerController : CachedMonoBehaviour
{
    [SerializeField]
    private GameObject m_AutographPrefab;
    [SerializeField]
    private float m_AutographRadius;
    [SerializeField]
    private float m_AutographDelay = 10f;
    private float m_AutographCooldown = 0f;

    private PlayerInputController m_Input;
    public PlayerInputController Input
	{
		get
		{
			if (m_Input == null)
			{
				m_Input = GetComponent<PlayerInputController>();
			}
			return m_Input;
		}
	}

    public bool LocomotionActive = true;
	private PlayerLocomotionController m_Locomotion;
	public PlayerLocomotionController Locomotion
	{
		get
		{
			if (m_Locomotion == null)
			{
				m_Locomotion = GetComponent<PlayerLocomotionController>();
			}
			return m_Locomotion;
		}
	}

    public bool AnimationActive = true;
	private PlayerAnimationController m_Animation;
    public PlayerAnimationController Animation
	{
		get
		{
			if (m_Animation == null)
			{
				m_Animation = GetComponent<PlayerAnimationController>();
			}
			return m_Animation;
		}
	}

	public bool IsInputBlocked { get; private set; }

	private static int M_LAYER = -1;
	public static int LAYER
	{
		get
		{
			if (M_LAYER == -1)
			{
				M_LAYER = LayerMask.NameToLayer("Player");
			}
			return M_LAYER;
		}
	}

	private void Awake()
	{
		Reset();
    }

	private void OnDestroy()
	{
		Input.Uninitialize();
		Locomotion.Uninitialize();
		Animation.Uninitialize();
	}

	private void Update()
    {
		if (GameManager.Instance.IsGameOver)
		{
			return;
		}

        if (!IsInputBlocked)
        {
            Input.UpdateBehaviour();
        }

        if (LocomotionActive)
        {
            Locomotion.UpdateBehaviour();
        }
        if (AnimationActive)
        {
            Animation.UpdateBehaviour();
        }

        m_AutographCooldown -= GameManager.Instance.DeltaTime;
        UIManager.Instance.UpdateUICoolDown(GetAutographNorm());
        if ( Input.ShootAutograph && m_AutographCooldown < 0f)
        {
            ShootAutograph();
        }
    }

    public void Reset()
    {
        Input.Initialize(this);
        Locomotion.Initialize(this);
		Animation.Initialize(this);

        m_AutographCooldown = 0f;
        UIManager.Instance.UpdateUICoolDown(GetAutographNorm());
    }

	public void SetInputLock(bool set)
	{
		IsInputBlocked = set;
	}

	public void ReceiveAttack()
	{
		Locomotion.AddMovementPenalty();
	}

    public void ShootAutograph()
    {
        if ( m_AutographPrefab)
        {
            Instantiate(m_AutographPrefab, CachedTransform.position, CachedTransform.rotation);
        }

        Collider[] girls = Physics.OverlapSphere(CachedTransform.position, m_AutographRadius, 1 << 9);
        int girlsNum = girls.Length;
        for (int i = 0; i < girlsNum; ++i)
        {
            GirlAI girlAI = girls[i].GetComponent<GirlAI>();
            girlAI.GetAutographed();
        }
        m_AutographCooldown = m_AutographDelay;
    }

    private float GetAutographNorm()
    {
        return Mathf.Clamp01(m_AutographCooldown / m_AutographDelay);
    }
}
