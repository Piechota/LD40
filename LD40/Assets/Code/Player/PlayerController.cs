
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerLocomotionController))]
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

	public bool IsInputBlocked { get; private set; }

	private void Awake()
	{
		Reset();
    }

	private void OnDestroy()
	{
		Input.Uninitialize();
		Locomotion.Uninitialize();
	}

	private void Update()
    {
        if (!IsInputBlocked)
        {
            Input.UpdateBehaviour();
        }

        Locomotion.UpdateBehaviour();
    }

    public void Reset()
    {
        Input.Initialize(this);
        Locomotion.Initialize(this);
    }
}
