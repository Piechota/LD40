using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPoint : CachedMonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        POIManager.Instance.RegisterNPCPoint(this);
    }
}
