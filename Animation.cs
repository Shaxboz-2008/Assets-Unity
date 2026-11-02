using UnityEngine;

public class TextureAnimStatic : MonoBehaviour
{
    public Texture[] frames;
    public float fps = 8f;
    private float timer;
    private int index;
    private Material mat;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    
    void Update()
    {
        if (frames == null || frames.Length == 0) return;
        
        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer = 0f;
            index = (index + 1) % frames.Length;
            mat.mainTexture = frames[index];
        }
    }
}