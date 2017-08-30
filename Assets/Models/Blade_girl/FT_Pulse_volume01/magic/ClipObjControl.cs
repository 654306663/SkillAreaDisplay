using UnityEngine;
using System.Collections;

public class ClipObjControl : MonoBehaviour {

    public Transform clipObjTrans;
    public SkinnedMeshRenderer skinRenderer;
	public SkinnedMeshRenderer skinRenderer1;

    private Material clipMaterial;
	private Material clipMaterial1;

	public Vector3 clipPos;
    private Vector3 clipNormal;

    void Start() {
		if (skinRenderer == null||skinRenderer == null)
            return;

        clipMaterial = skinRenderer.sharedMaterial;
		clipMaterial1 = skinRenderer1.sharedMaterial;

		//clipPos = clipObjTrans.position;
    }

    void SetMaterialValue(Vector3 pos, Vector3 normal)
    {
		if (clipMaterial == null||clipMaterial1 == null)
            return;

        clipMaterial.SetVector("_ClipObjPos", pos);
        clipMaterial.SetVector("_ClipObjNormal", normal);
		clipMaterial1.SetVector("_ClipObjPos", pos);
		clipMaterial1.SetVector("_ClipObjNormal", normal);
    }
	
	void Update () {
	    
        if (clipObjTrans == null || clipMaterial == null)
            return;

        clipPos = clipObjTrans.position;

        //计算XY平面上的法向量，用XY平面作为剔除面
		clipNormal = clipObjTrans.rotation * Vector3.down;

        SetMaterialValue(clipPos, clipNormal);
	}
}
