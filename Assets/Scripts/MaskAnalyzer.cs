using UnityEngine;
using TMPro;

public class MaskAnalyzer : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private TMP_Text percentageText;
    [SerializeField] private float analyzeInterval = 1f;

    private float timer = 0f;
    private Mesh mesh;

    void Awake()
    {
        mesh = meshFilter.mesh;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= analyzeInterval)
        {
            AnalyzeVertexFoamAndCleanPercentage();
            timer = 0f;
        }
    }

    public void AnalyzeVertexFoamAndCleanPercentage()
    {
        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogWarning("MeshFilter atanmamış.");
            return;
        }

        Color[] colors = mesh.colors;

        if (colors == null || colors.Length == 0)
        {
            Debug.LogWarning("Mesh vertex color içermiyor.");
            return;
        }

        int total = colors.Length;
        int foamCount = 0;
        int cleanCount = 0;

        foreach (Color c in colors)
        {
            float r = c.r;

            if (r >= 0.5f && r < 1.0f)
                foamCount++;

            if (r >= 1.0f)
                cleanCount++;
        }

        float foamPercent = (float)foamCount / total * 100f;
        float cleanPercent = (float)cleanCount / total * 100f;

        percentageText.text = $"Foam: %{foamPercent:F1}  Clean: %{cleanPercent:F1}";
        Debug.Log($"[Vertex Color] Foam: %{foamPercent:F1}, Clean: %{cleanPercent:F1}");
    }
}