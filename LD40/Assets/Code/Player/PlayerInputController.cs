using UnityEngine;

public class PlayerInputController : APlayerComponent
{
	public Vector3 InputVector { private set; get; }

	private const string HORIZONTAL_AXIS = "Horizontal";
	private const string VERTICAL_AXIS = "Vertical";

	protected override void HandleInitialization(PlayerController player)
	{
	}

	protected override void HandleUninitialization()
	{
	}

	public override void UpdateBehaviour()
	{
		base.UpdateBehaviour();

		UpdateInput();
	}

	private void UpdateInput()
	{
		InputVector = new Vector3(Input.GetAxis(HORIZONTAL_AXIS), 0, Input.GetAxis(VERTICAL_AXIS));
		if (InputVector.magnitude > 1)
		{
			InputVector = InputVector.normalized;
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if (!Application.isPlaying)
		{
			return;
		}

		Color prevColor = Gizmos.color;
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(CachedTransform.position, CachedTransform.position + InputVector);
		}
		Gizmos.color = prevColor;
	}
#endif
}
