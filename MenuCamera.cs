using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Camera menuCamera;
    public GameObject player;

    void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        if (menuCamera != null) menuCamera.gameObject.SetActive(true);
        if (player != null) player.SetActive(false);
    }

    public void StartGame()
    {
        if (menuCamera != null) menuCamera.gameObject.SetActive(false);
        if (player != null) player.SetActive(true);
    }
}