using UnityEngine;
using TMPro;

public class MaskAnalyzer : MonoBehaviour
{
    [SerializeField] private RenderTexture maskRT;
    [SerializeField] private int downscale = 4; // Daha hızlı analiz için
    [SerializeField] private TMP_Text percentageText;

    private float analyzeInterval = 1f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= analyzeInterval)
        {
            AnalyzeCleanPercentage();
            timer = 0f;
        }
    }

    public void AnalyzeCleanPercentage()
    {
        int width = maskRT.width / downscale;
        int height = maskRT.height / downscale;

        Texture2D tempTex = new Texture2D(width, height, TextureFormat.RGB24, false, true);

        RenderTexture activeRT = RenderTexture.active;
        RenderTexture.active = maskRT;

        // Küçük boyutla sadece merkez alanı oku
        tempTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tempTex.Apply();

        RenderTexture.active = activeRT;

        int total = width * height;
        int cleanCount = 0;

        Color[] pixels = tempTex.GetPixels();

        foreach (Color pixel in pixels)
        {
            float r = pixel.r; // sadece R kanalı kullanılıyor
            if (r >= 0.66f) // clean olarak kabul ettiğimiz eşik
                cleanCount++;
        }

        float percent = (float)cleanCount / total * 100f;
        percentageText.text = $"Clean Rate: %{percent:F1}";
        Debug.Log($"Temizlik: %{percent:F1}");
    }
}