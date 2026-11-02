using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectE : MonoBehaviour
{
    public float interactionDistance = 3.0f;
    public PromtE promtObject;
    
    private Transform player;
    private bool playerInRange = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (promtObject != null) promtObject.Hide();
    }
    
    void Update()
    {
        if (player == null || promtObject == null) return;
        
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance <= interactionDistance && !playerInRange)
        {
            playerInRange = true;
            promtObject.Show();
        }
        else if (distance > interactionDistance && playerInRange)
        {
            playerInRange = false;
            promtObject.Hide();
        }
        
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log($"Взаимодействие с {gameObject.name}");
        }
    }
}