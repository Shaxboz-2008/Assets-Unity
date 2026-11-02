using UnityEngine;

public class AutoTiling : MonoBehaviour
{
    public Texture2D texture;

    void OnValidate()
    {
        if (texture == null) return;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        Material mat = renderer.sharedMaterial;
        mat.mainTexture = texture;
        mat.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.z);
    }
}