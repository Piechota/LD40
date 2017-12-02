public abstract class APlayerComponent : CachedMonoBehaviour
{
	protected PlayerController m_Player;

	public bool IsInitialized { get; private set; }

	public void Initialize(PlayerController player)
	{
		if (IsInitialized)
		{
			Uninitialize();
		}

		m_Player = player;
		HandleInitialization(player);
		IsInitialized = true;
	}

	protected abstract void HandleInitialization(PlayerController player);

	public virtual void UpdateBehaviour()
	{
	}

	public void Uninitialize()
	{
		if (IsInitialized)
		{
			HandleUninitialization();
			IsInitialized = false;
		}
	}

	protected abstract void HandleUninitialization();
}
