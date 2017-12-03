using UnityEngine;

[CreateAssetMenu(fileName = "FanParams", menuName = "", order = 1)]
public class FanParams : ScriptableObject
{
	[Header("Idle")]
	public float WalkSpeed = 4f;
	public float SpotSpeed = 4f;
	public float IdleRotationSpeed = 11f;
	public Vector2 IdleRotationDelay = new Vector2(3f, 5f);
	public Vector2 IdleRandomTime = new Vector2(6f, 10f);

	[Header("Spotted")]
	public float RunSpeed = 11f;
	public float SpottedAttackDistance = 1.5f;

    [Header("Shout")]
    public float ShoutRadius = 10f;
    public float ShoutTime = 1f;

    [Header("Autographed")]
    public Vector2 AutographedTime = new Vector2(3f, 5f);
    public Vector2 AutographedRotationDelay = new Vector2(0.2f, 0.5f);
    public float AutographedRotationSpeed = 20f;
}