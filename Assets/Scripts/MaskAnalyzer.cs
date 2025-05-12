using UnityEngine;
using TMPro;

public class MaskAnalyzer : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private TMP_Text percentageText;
    [SerializeField] private float analyzeInterval = 1f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= analyzeInterval)
        {
            AnalyzeVertexCleanPercentage();
            timer = 0f;
        }
    }

    public void AnalyzeVertexCleanPercentage()
    {
        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogWarning("MeshFilter atanmamış.");
            return;
        }

        Mesh mesh = meshFilter.mesh;
        Color[] colors = mesh.colors;

        if (colors == null || colors.Length == 0)
        {
            Debug.LogWarning("Mesh vertex color içermiyor.");
            return;
        }

        int total = colors.Length;
        int cleanCount = 0;

        foreach (Color c in colors)
        {
            if (c.r >= 0.66f) // temiz kabul edilen eşik
                cleanCount++;
        }

        float percent = (float)cleanCount / total * 100f;
        percentageText.text = $"Clean Rate: %{percent:F1}";
        Debug.Log($"[Vertex Color] Temizlik: %{percent:F1}");
    }
}