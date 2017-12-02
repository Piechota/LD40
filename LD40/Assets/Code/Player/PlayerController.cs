
using System.Collections.Generic;
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

	private List<GirlAI> m_PickupOptions = new List<GirlAI>();
	private GirlAI m_CurrentFollower;
	public bool IsEscorting { get { return m_CurrentFollower != null; } }

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
        if (!IsInputBlocked)
        {
            Input.UpdateBehaviour();
        }

		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))		// #TODO LS update this
		{
			if (m_CurrentFollower == null)
			{
				if (m_PickupOptions.Count != 0)
				{
					GirlAI closest = GetClosestPickupOption();
					m_CurrentFollower = closest;
					closest.StartPair();
				}
			}
			else
			{
				m_CurrentFollower.StopPair();
				m_CurrentFollower = null;
			}
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

	public void AddPickupOption(GirlAI girl)
	{
		if (!m_PickupOptions.Contains(girl))
		{
			m_PickupOptions.Add(girl);
		}
	}

	public void RemovePickupOption(GirlAI girl)
	{
		m_PickupOptions.Remove(girl);
	}

	private GirlAI GetClosestPickupOption()
	{
		GirlAI closest = m_PickupOptions[0];
		float closestDist = Vector3.Distance(closest.transform.position, CachedTransform.position);

		for (int i = 1; i < m_PickupOptions.Count; ++i)
		{
			GirlAI girl = m_PickupOptions[i];
			float dist = Vector3.Distance(girl.transform.position, CachedTransform.position);

			if (dist < closestDist)
			{
				closest = girl;
				closestDist = dist;
			}
		}

		return closest;
	}

	public void PerformDate(Location loc)
	{
		GirlAI follower = m_CurrentFollower;
		m_CurrentFollower = null;
		follower.DateFinished();
	}

	public void SetInputLock(bool set)
	{
		IsInputBlocked = set;
	}
}
