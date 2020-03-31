using UnityEngine;

[ExecuteInEditMode]
public class CelLightDirection : MonoBehaviour
{
    private void Update() => Shader.SetGlobalVector("_LightDir", -transform.forward);
}