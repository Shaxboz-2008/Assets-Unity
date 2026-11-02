using UnityEngine;
using System.Collections.Generic;

public class PromtE : MonoBehaviour
{
    public List<Texture> frames = new List<Texture>();
    public float fps = 6f;
    public float distanceFromCamera = 1.5f;
    
    private Material mat;
    private Transform cam;
    private float timer;
    private int index;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        cam = Camera.main.transform;
    }
    
    void Update()
    {
        if (cam == null || frames.Count == 0) return;
        
        transform.position = cam.position + cam.forward * distanceFromCamera;
        transform.rotation = cam.rotation;
        
        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer = 0f;
            index = (index + 1) % frames.Count;
            mat.mainTexture = frames[index];
        }
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}