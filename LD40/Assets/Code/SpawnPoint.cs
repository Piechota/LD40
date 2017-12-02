using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : CachedMonoBehaviour {

    public bool IsUsed { get; set; }

	void Start () {
        GirlsManager.Instance.RegisterSpawnPoint(this);
        IsUsed = false;
    }
}
