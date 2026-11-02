using UnityEngine;
using System.Collections;

public class StartGameButton : MenuButton
{
    public MenuController menuController;

    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 1.2f;
    public Color startColor = Color.white;
    public Color middleColor = new Color(1f, 0.75f, 0.8f);
    public Color endColor = new Color(1f, 0.6f, 0.3f);

    public System.Action onClick;

    private Texture2D fadeTexture;
    private bool fading;
    private Color currentColor = Color.white;
    private float currentAlpha;

    public override void Activate()
    {
        onClick?.Invoke();

        StartCoroutine(FadeAndStart());
    }

    IEnumerator FadeAndStart()
    {
        fading = true;

        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, Color.white);
        fadeTexture.Apply();

        yield return StartCoroutine(FadeToOpaque());

        if (menuController != null)
            menuController.StartGame();

        yield return StartCoroutine(FadeToClear());
        fading = false;
    }

    IEnumerator FadeToOpaque()
    {
        float elapsed = 0f;
        currentAlpha = 0f;
        currentColor = startColor;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInDuration;
            currentAlpha = Mathf.SmoothStep(0f, 1f, t);
            currentColor = Color.Lerp(startColor, middleColor, t);
            currentColor.a = currentAlpha;
            yield return null;
        }
    }

    IEnumerator FadeToClear()
    {
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            currentAlpha = Mathf.SmoothStep(1f, 0f, t);
            currentColor = Color.Lerp(middleColor, endColor, t);
            currentColor.a = currentAlpha;
            yield return null;
        }
    }

    void OnGUI()
    {
        if (!fading) return;

        GUI.color = currentColor;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }
}