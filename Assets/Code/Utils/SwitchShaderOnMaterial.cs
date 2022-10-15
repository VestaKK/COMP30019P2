using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchShaderOnMaterial : MonoBehaviour
{
    [SerializeField] Enemy _enemy;
    public SkinnedMeshRenderer[] SMR;
    public MeshRenderer[] MR;

    private bool LockedOn = false;

    private void OnEnable()
    {
        SMR = this.GetComponentsInChildren<SkinnedMeshRenderer>();
        MR = this.GetComponentsInChildren<MeshRenderer>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            SwitchShaders();
        }
    }

    private void SwitchShaders() 
    {
        if (LockedOn)
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

            LockedOn = false;
        }
        else
        {
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
            LockedOn = true;
        }
    }
}
