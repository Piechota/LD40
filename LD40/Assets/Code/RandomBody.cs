using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBody : CachedMonoBehaviour {

    [Header("Hair")]
    [SerializeField]
    private Renderer m_Hair;
    [SerializeField]
    private Gradient m_HairColors;

    [Header("Body")]
    [SerializeField]
    private Renderer m_Body;
    [SerializeField]
    private Gradient m_BodyColors;

    [Header("Skin")]
    [SerializeField]
    private Renderer m_Skin;
    [SerializeField]
    private Gradient m_SkinColors;

    [Header("Eyes")]
    [SerializeField]
    private Renderer m_Eyes;
    [SerializeField]
    private Gradient m_EyesColors;

    // Use this for initialization
    void Start () {
        m_Hair.material.color = m_HairColors.Evaluate(Random.value);
        m_Body.material.color = m_BodyColors.Evaluate(Random.value);
        m_Skin.material.color = m_SkinColors.Evaluate(Random.value);
        m_Eyes.material.color = m_EyesColors.Evaluate(Random.value);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
