using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAI : CachedMonoBehaviour
{
    public Material ConeMaterial;
    private bool Catched;
    // Use this for initialization
    void Start() {

    }

    private void DrawCone( Color color )
    {
        const int triangleNum = 20;

        float deltaDegree = 2f * GameManager.Instance.FieldOfView.ConeDegree * Mathf.Deg2Rad / (float)triangleNum;
        float cosDelta = Mathf.Cos(deltaDegree);
        float sinDelta = Mathf.Sin(deltaDegree);

        float coneCos = Mathf.Cos(-GameManager.Instance.FieldOfView.ConeDegree * Mathf.Deg2Rad);
        float coneSin = Mathf.Sin(-GameManager.Instance.FieldOfView.ConeDegree * Mathf.Deg2Rad);

        Vector3 initDir = new Vector3(coneSin * CachedTransform.forward.z + coneCos * CachedTransform.forward.x, 0f, coneCos * CachedTransform.forward.z - coneSin * CachedTransform.forward.x);
        initDir.Normalize();
        Vector3 pos0 = CachedTransform.position;
        float coneRadius = GameManager.Instance.FieldOfView.RaysDistance;
        GL.Begin(GL.TRIANGLES);
        ConeMaterial.SetPass(0);
        ConeMaterial.color = color;
        GL.Color(Color.white);

        for (int i = 0; i < triangleNum; ++i)
        {
            GL.Vertex3(pos0.x, pos0.y, pos0.z);
            GL.Vertex3(pos0.x + initDir.x * coneRadius, pos0.y + initDir.y * coneRadius, pos0.z + initDir.z * coneRadius);

            float x = sinDelta * initDir.z + cosDelta * initDir.x;
            float z = cosDelta * initDir.z - sinDelta * initDir.x;
            initDir.x = x;
            initDir.z = z;

            GL.Vertex3(pos0.x + initDir.x * coneRadius, pos0.y + initDir.y * coneRadius, pos0.z + initDir.z * coneRadius);
        }
        GL.End();
    }
    void OnRenderObject()
    {
        DrawCone(Catched ? Color.red : Color.blue);
    }
    // Update is called once per frame
    void Update ()
    {
        Catched = GameManager.Instance.FieldOfView.TestCollision(CachedTransform.position, CachedTransform.forward);
	}
}
