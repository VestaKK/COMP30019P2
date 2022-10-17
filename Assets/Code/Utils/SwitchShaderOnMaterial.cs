using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchShaderOnMaterial : MonoBehaviour
{
    [SerializeField] Mob _mob;
    public SkinnedMeshRenderer[] SMR;
    public MeshRenderer[] MR;

    //public Dictionary<SkinnedMeshRenderer, Material[]> SMRmats = new Dictionary<SkinnedMeshRenderer, Material[]>();
    //public Dictionary<MeshRenderer, Material[]> MRmats = new Dictionary<MeshRenderer, Material[]>();

    private void OnEnable()
    {
        _mob = GetComponent<Mob>();
        _mob.LockedOn.AddListener(SwitchToOutline);
        _mob.LockedOff.AddListener(SwitchToNormal);
        SMR = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        MR = this.GetComponentsInChildren<MeshRenderer>();
    }

    private void OnDisable()
    {
        _mob.LockedOn.RemoveListener(SwitchToOutline);
        _mob.LockedOff.RemoveListener(SwitchToNormal);
    }

    private void SwitchToNormal() 
    {
        foreach (SkinnedMeshRenderer smr in SMR)
        {
            Material[] materials = smr.materials;

            foreach (Material mat in materials)
            {
                mat.shader = Shader.Find("PUNKSOULS/Cel");
                mat.renderQueue = 2000;
            }

            smr.materials = materials;
        }

        foreach (MeshRenderer mr in MR)
        {
            Material[] materials = mr.materials;

            foreach (Material mat in materials)
            {
                mat.shader = Shader.Find("PUNKSOULS/Cel");
                mat.renderQueue = 2000;
            }

            mr.materials = materials;
        }
    }

    private void SwitchToOutline() {
        foreach (SkinnedMeshRenderer smr in SMR)
        {
            Material[] materials = smr.materials;

            foreach (Material mat in materials)
            {

                mat.shader = Shader.Find("PUNKSOULS/CelOutline");
                mat.SetFloat("_OutlineWidth", 0.03f);
                mat.SetFloat("_Angle", 180);
                mat.renderQueue = 3000;
            }
            smr.materials = materials;
        }

        foreach (MeshRenderer mr in MR)
        {
            Material[] materials = mr.materials;

            foreach (Material mat in materials)
            {
                mat.shader = Shader.Find("PUNKSOULS/CelOutline");
                mat.SetFloat("_OutlineWidth", 0.03f);
                mat.SetFloat("_Angle", 180);
                mat.renderQueue = 3000;
            }
            mr.materials = materials;
        }
    }
}
