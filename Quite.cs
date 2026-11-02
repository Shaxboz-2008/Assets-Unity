using UnityEngine;

public class QuitButton : MonoBehaviour
{
    void OnMouseDown()
    {
        Application.Quit();
    }
}