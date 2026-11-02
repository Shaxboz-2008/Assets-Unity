using UnityEngine;

public class TextureAnim : MonoBehaviour
{
    public Texture[] frames;
    public float fps = 8f;
    public Transform player;
    public float rotationSpeed = 5f;
    public float followDistance = 10f;
    private float timer;
    private int index;
    private Material mat;
    private Quaternion defaultRotation;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        defaultRotation = transform.rotation;
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
        
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            
            if (distance <= followDistance)
            {
                Vector3 direction = transform.position - player.position;
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}