
using UnityEngine;
using DentedPixel;

public class Dissolvable : MonoBehaviour {

    MeshRenderer meshRenderer;
    Material material;
    Collider col;
    string dissolveReference = "Vector1_37B9DF73";

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        col = GetComponent<Collider>();
    }

	public void Dissolve(float time)
    {
        if (meshRenderer != null)
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        if(material != null)
        {
            LeanTween.value(0, 1, time).setOnUpdate(
                (float x) =>
                {
                    material.SetFloat(dissolveReference, x);
                });
        }

        if(col != null)
        {
            col.enabled = false;
        }
    }

    public void Reset()
    {
        if (meshRenderer != null)
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        if (material != null)
        {
            material.SetFloat(dissolveReference, 0);
        }
        if (col != null)
        {
            col.enabled = true;
        }
    }

    public void SetDissolve()
    {
        if (meshRenderer != null)
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        if (material != null)
        {
            material.SetFloat(dissolveReference, 1);
        }
        if (col != null)
        {
            col.enabled = false;
        }
    }
}
