using UnityEngine;
using UnityEngine.InputSystem;

public class MenuCameraMouseMove : MonoBehaviour
{
    public float moveAmount = 0.1f;
    public float smoothSpeed = 5f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        float mouseX = mousePos.x / Screen.width - 0.5f;
        float mouseY = mousePos.y / Screen.height - 0.5f;

        Vector3 targetPosition = startPosition + new Vector3(mouseX * moveAmount, mouseY * moveAmount, 0f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}