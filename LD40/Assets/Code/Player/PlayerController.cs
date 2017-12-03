using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerLocomotionController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerController : CachedMonoBehaviour
{
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

        Locomotion.UpdateBehaviour();
        Animation.UpdateBehaviour();
    }

    public void Reset()
    {
        Input.Initialize(this);
        Locomotion.Initialize(this);
		Animation.Initialize(this);
    }

	public void SetInputLock(bool set)
	{
		IsInputBlocked = set;
	}
}
