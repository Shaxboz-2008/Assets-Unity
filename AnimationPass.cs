using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MenuButton : MonoBehaviour
{
    [Header("Кадры")]
    public List<Texture> idleFrames = new List<Texture>();
    public List<Texture> hoverFrames = new List<Texture>();
    public List<Texture> clickFrames = new List<Texture>();

    [Header("Анимация")]
    public float fps = 8f;

    [Header("Звук")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private Material mat;
    private float timer;
    private int frameIndex;
    private bool isHovered;
    private bool isClicked;
    private Vector3 originalPosition;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalPosition = transform.localPosition;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        Animate();
    }

    void Animate()
    {
        List<Texture> currentFrames;

        if (isClicked && clickFrames.Count > 0)
            currentFrames = clickFrames;
        else if (isHovered && hoverFrames.Count > 0)
            currentFrames = hoverFrames;
        else
            currentFrames = idleFrames;

        if (currentFrames.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer = 0f;
            frameIndex = (frameIndex + 1) % currentFrames.Count;
            mat.mainTexture = currentFrames[frameIndex];
        }
    }

    void OnMouseEnter()
    {
        isHovered = true;
        transform.localPosition = originalPosition + new Vector3(0f, 0f, 0.05f);
        if (audioSource != null && hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    void OnMouseExit()
    {
        isHovered = false;
        transform.localPosition = originalPosition;
    }

    void OnMouseDown()
    {
        isClicked = true;
        transform.localPosition = originalPosition + new Vector3(0f, 0f, -0.05f);
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
        Activate();
        CancelInvoke("ResetClick");
        Invoke("ResetClick", 0.5f);
    }

    void ResetClick()
    {
        isClicked = false;
        transform.localPosition = originalPosition;
    }

    public virtual void Activate()
    {
        Debug.Log("Кнопка нажата: " + gameObject.name);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}