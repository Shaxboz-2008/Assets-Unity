using UnityEngine;
using UnityEngine.InputSystem;

public class FPC : MonoBehaviour
{
    public Transform playerCamera;
    
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 2.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    
    public float mouseSensitivity = 0.1f;
    public float verticalLookLimit = 80f;
    
    public float bobSpeed = 10f;
    public float bobAmountX = 0.05f;
    public float bobAmountY = 0.03f;
    
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchTransitionSpeed = 10f;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;
    private float defaultCamY;
    private float bobTimer;
    private float verticalRotation;
    
    private Keyboard keyboard;
    private Mouse mouse;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        defaultCamY = playerCamera.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        keyboard = Keyboard.current;
        mouse = Mouse.current;
    }
    
    void Update()
    {
        if (keyboard == null || mouse == null) return;
        
        float mouseX = mouse.delta.x.ReadValue() * mouseSensitivity;
        float mouseY = mouse.delta.y.ReadValue() * mouseSensitivity;
        
        transform.Rotate(Vector3.up * mouseX);
        
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        
        float x = 0f;
        float z = 0f;
        
        if (keyboard.wKey.isPressed) z += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;
        if (keyboard.dKey.isPressed) x += 1f;
        if (keyboard.aKey.isPressed) x -= 1f;
        
        Vector3 move = transform.right * x + transform.forward * z;
        move.Normalize();
        
        bool sprint = keyboard.leftShiftKey.isPressed && !isCrouching;
        float currentSpeed = isCrouching ? crouchSpeed : (sprint ? sprintSpeed : walkSpeed);
        
        controller.Move(move * currentSpeed * Time.deltaTime);
        
        if (keyboard.spaceKey.wasPressedThisFrame && isGrounded && !isCrouching)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        if (keyboard.leftCtrlKey.wasPressedThisFrame)
            isCrouching = !isCrouching;
        
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        
        float cameraTargetY = isCrouching ? defaultCamY - 0.5f : defaultCamY;
        
        bool isMoving = move.magnitude > 0.1f && isGrounded;
        
        if (isMoving)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float bobX = Mathf.Sin(bobTimer) * bobAmountX;
            float bobY = Mathf.Sin(bobTimer * 2) * bobAmountY;
            float intensity = currentSpeed / sprintSpeed;
            bobX *= intensity;
            bobY *= intensity;
            
            playerCamera.localPosition = new Vector3(bobX, cameraTargetY + bobY, playerCamera.localPosition.z);
        }
        else
        {
            bobTimer = 0f;
            playerCamera.localPosition = new Vector3(0f, cameraTargetY, playerCamera.localPosition.z);
        }
    }
}