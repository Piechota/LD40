using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : CachedMonoBehaviour
{
    public enum ETag
    {
        None,
        Food,
        Cinema,
        Shop
    };
    public const int TagsNum = (int)ETag.Shop + 1;

    public ETag[] Tags;
    public bool IsUsed { get; set; }

	void Start () {
        POIManager.Instance.RegisterSpawnPoint(this);
        IsUsed = false;
    }
}
