using UnityEngine;

public class BoatRide : MonoBehaviour
{
    [Header("Кнопка старта (Quad со StartGameButton)")]
    public GameObject startButton;

    [Header("Движение лодки")]
    public Transform targetLocation;
    public float speed = 2f;
    public float slowdownTime = 1.5f;

    [Header("Поворот лодки")]
    public float forwardAngleY = 270f;

    [Header("Причаливание боком")]
    public float dockingAngle = 90f;
    public float dockingTurnDistance = 5f;

    [Header("Покачивание (качка)")]
    public float bobAmplitude = 0.1f;
    public float bobSpeed = 1.5f;
    public float tiltAmplitude = 3f;
    public float tiltSpeed = 1f;

    [Header("Игрок")]
    public GameObject player;
    public MonoBehaviour[] playerScripts;

    private Vector3 playerOffset;
    private Quaternion playerRotOffset;
    private float bobTimer = 0f;
    private float baseY;
    private bool rideStarted = false;
    private bool isMoving = false;
    private bool isSlowingDown = false;
    private bool isDocking = false;
    private float slowdownTimer = 0f;
    private float currentSpeed;
    private Quaternion dockingStartRotation;
    private Quaternion targetDockingRotation;
    private float dockingStartDistance;
    private Quaternion forwardRotation;

    void Start()
    {
        baseY = transform.position.y;
        currentSpeed = speed;
        forwardRotation = Quaternion.Euler(0f, forwardAngleY, 0f);

        // Лодка стоит, пока не нажата кнопка
        isMoving = false;

        // Подписываемся на событие кнопки Start
        if (startButton != null)
        {
            var btn = startButton.GetComponent<StartGameButton>();
            if (btn != null)
                btn.onClick += StartRide;
            else
                Debug.LogWarning("На startButton нет скрипта StartGameButton");
        }
        else
        {
            Debug.LogWarning("BoatRide: не назначена startButton — лодка не поедет.");
        }
    }

    /// <summary>
    /// Вызывается из StartGameButton при клике.
    /// </summary>
    public void StartRide()
    {
        if (rideStarted) return;
        rideStarted = true;

        // Запоминаем offset игрока относительно лодки именно сейчас
        if (player != null)
        {
            playerOffset = transform.InverseTransformPoint(player.transform.position);
            playerRotOffset = Quaternion.Inverse(transform.rotation) * player.transform.rotation;
        }

        // Замораживаем игрока
        SetPlayerScripts(false);

        // Запускаем движение
        isMoving = true;

        Debug.Log("Start нажат — лодка поплыла!");
    }

    void Update()
    {
        if (!isMoving || targetLocation == null) return;

        Vector3 flatBoat = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatTarget = new Vector3(targetLocation.position.x, 0, targetLocation.position.z);
        float distanceLeft = Vector3.Distance(flatBoat, flatTarget);

        if (distanceLeft < currentSpeed * slowdownTime && !isSlowingDown)
            isSlowingDown = true;

        if (isSlowingDown)
        {
            slowdownTimer += Time.deltaTime;
            currentSpeed = Mathf.Lerp(speed, 0.2f, slowdownTimer / slowdownTime);
        }

        Vector3 newFlatPos = Vector3.MoveTowards(flatBoat, flatTarget, currentSpeed * Time.deltaTime);
        transform.position = new Vector3(newFlatPos.x, baseY, newFlatPos.z);

        bobTimer += Time.deltaTime * bobSpeed;
        float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, baseY + bobOffset, transform.position.z);

        float tiltOffset = Mathf.Sin(bobTimer * tiltSpeed) * tiltAmplitude;

        if (distanceLeft < dockingTurnDistance && !isDocking)
        {
            isDocking = true;
            dockingStartRotation = forwardRotation;
            dockingStartDistance = distanceLeft;
            targetDockingRotation = Quaternion.Euler(0f, forwardAngleY + dockingAngle, 0f);
        }

        if (isDocking)
        {
            float t = 1f - Mathf.Clamp01(distanceLeft / dockingStartDistance);
            Quaternion dockRot = Quaternion.Slerp(dockingStartRotation, targetDockingRotation, t);
            transform.rotation = dockRot * Quaternion.Euler(0, 0, tiltOffset);
        }
        else
        {
            transform.rotation = forwardRotation * Quaternion.Euler(0, 0, tiltOffset);
        }

        if (player != null)
        {
            player.transform.position = transform.TransformPoint(playerOffset);
            player.transform.rotation = transform.rotation * playerRotOffset;
        }

        if (distanceLeft < 0.1f)
        {
            isMoving = false;
            Arrive();
        }
    }

    void Arrive()
    {
        Debug.Log("Лодка причалила! Игрок разморожен.");
        SetPlayerScripts(true);
    }

    void SetPlayerScripts(bool state)
    {
        if (playerScripts == null) return;
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != null)
                script.enabled = state;
        }
    }
}