using UnityEngine;

[System.Serializable]
public class GirlFOV
{
	public int RaysNum;
	public float RaysDistance;
	public float ConeDegree;

	public bool TestCollision(Vector3 position, Vector3 lookDir)
	{
		if (GameManager.Instance.PlayerCollider != null)
		{
			float playerRadius = GameManager.Instance.PlayerCollider.radius;
			float coneDot = Mathf.Cos(ConeDegree * Mathf.Deg2Rad);
			Vector3 playerPosition = GameManager.Instance.Player.transform.position;
			Vector3 dir = playerPosition - position;
			if (Vector3.Distance(playerPosition - dir.normalized * playerRadius, position) <= RaysDistance)
			{
				Vector3 perpendicularDir = new Vector3(dir.z, 0.0f, -dir.x);
				perpendicularDir.Normalize();
				Vector3 testPosition = playerPosition + perpendicularDir * playerRadius;
				RaycastHit hitInfo;
				float delta = (2.0f * playerRadius) / (float)RaysNum;
				for (int i = 0; i < RaysNum; ++i)
				{
					dir = testPosition - position;
					dir.Normalize();

					if (coneDot < Vector3.Dot(dir, lookDir))
					{
						if (Physics.Raycast(position, dir, out hitInfo, RaysDistance))
						{
							if (hitInfo.transform.gameObject.layer == GameManager.Instance.Player.gameObject.layer)
							{
								return true;
							}
						}
					}
					testPosition -= perpendicularDir * delta;
				}
			}
		}
		return false;
	}
}