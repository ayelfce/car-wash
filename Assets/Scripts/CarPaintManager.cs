using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CarPaintManager : MonoBehaviour
{
    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        if (mesh.colors == null || mesh.colors.Length != mesh.vertexCount)
        {
            Color[] colors = new Color[mesh.vertexCount];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.black; // Çamurlu başlasın
            mesh.colors = colors;
        }
    }

    public void ResetToMud()
    {
        if (mesh == null) mesh = GetComponent<MeshFilter>().mesh;

        Color[] colors = new Color[mesh.vertexCount];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = Color.black;

        mesh.colors = colors;
    }

    public Mesh GetMesh() => mesh;
}