using UnityEngine;

public class SprayBrushPainter : MonoBehaviour
{
    public RenderTexture targetMask;
    public Material brushMaterial;
    public float brushSize = 0.05f;

    private void Start()
    {
        ClearMaskTexture(targetMask);
    }

    public void PaintAt(Vector2 uv, SprayEmitter.SprayType type)
    {
        if (targetMask == null || brushMaterial == null) return;

        // Foam = 0.5, Water = 1.0
        Color brushColor = type == SprayEmitter.SprayType.Foam
    ? new Color(0.4f, 0.4f, 0.4f, 1f) // maskValue = 0.4 → foam görünür
    : new Color(1.0f, 1.0f, 1.0f, 1f); // maskValue = 1.0 → doğrudan clean

        brushMaterial.SetVector("_Coord", new Vector4(uv.x, uv.y, 0, 0));
        brushMaterial.SetFloat("_Size", brushSize);
        brushMaterial.SetColor("_Color", brushColor);

        RenderTexture activeRT = RenderTexture.active;
        RenderTexture.active = targetMask;

        Graphics.Blit(null, targetMask, brushMaterial);

        RenderTexture.active = activeRT;
    }

    public void ClearMaskTexture(RenderTexture rt)
    {
        if (rt == null) return;

        RenderTexture active = RenderTexture.active;
        RenderTexture.active = rt;
        GL.Clear(true, true, Color.black); // Başlangıçta tamamen çamurlu
        RenderTexture.active = active;
    }
}