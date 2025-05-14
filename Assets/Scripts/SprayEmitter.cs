using UnityEngine;

public class SprayEmitter : MonoBehaviour
{
    public enum SprayType { Foam, Water }
    [SerializeField] private SprayType sprayType;

    [SerializeField] private float sprayRange = 10f;
    [SerializeField] private float sprayRadius = 0.2f;
    [SerializeField] private LayerMask hitLayers;

    [SerializeField] private ParticleSystem sprayParticle;
    [SerializeField] private float brushSpeed = 1f;

    private Transform rayOrigin;
    private bool isSprayerActive => SprayerManager.Instance != null && SprayerManager.Instance.GetActiveSprayer() == transform.parent;

    void Start()
    {
        rayOrigin = transform.Find("RayOrigin");

        if (rayOrigin == null)
            Debug.LogWarning("RayOrigin child objesi bulunamadı!");

        if (sprayParticle != null && sprayParticle.isPlaying)
            sprayParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void Update()
    {
        if (!isSprayerActive) return;

        bool sprayActive = Input.GetMouseButton(1); // Sağ tık

        if (sprayActive)
        {
            if (!sprayParticle.isEmitting)
            {
                sprayParticle.Clear();
                sprayParticle.Play();
            }

            Spray();
        }
        else
        {
            if (sprayParticle.isEmitting)
            {
                sprayParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    void Spray()
    {
        if (rayOrigin == null) return;

        Vector3 origin = rayOrigin.position;
        Vector3 direction = (rayOrigin.forward + (-rayOrigin.up * 0.3f)).normalized;

        RaycastHit[] hits = Physics.SphereCastAll(origin, sprayRadius, direction, sprayRange, hitLayers);

        int maxPaints = 5;
        int painted = 0;

        foreach (RaycastHit hit in hits)
        {
            if (painted >= maxPaints) break;

            if (hit.collider.CompareTag("Car"))
            {
                CarPaintManager carPaintManager = hit.collider.GetComponent<CarPaintManager>();
                if (carPaintManager != null)
                {
                    Mesh mesh = carPaintManager.GetMesh();
                    Vector3[] verts = mesh.vertices;
                    Color[] colors = mesh.colors;
                    Transform carTransform = carPaintManager.transform;

                    ApplyVertexBrush(verts, colors, carTransform, hit.point);
                    mesh.colors = colors;
                    painted++;
                }
            }
        }
    }
    void ApplyVertexBrush(Vector3[] vertices, Color[] colors, Transform carTransform, Vector3 worldHitPoint)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = carTransform.TransformPoint(vertices[i]);
            float dist = Vector3.Distance(worldHitPoint, worldPos);

            if (dist < sprayRadius)
            {
                float softness = 1f - (dist / sprayRadius);
                softness = Mathf.Clamp(softness, 0f, 1f);

                float effect = brushSpeed * Time.deltaTime * softness;

                float current = colors[i].r;

                if (sprayType == SprayType.Foam)
                    colors[i].r = Mathf.Clamp(current + effect, 0f, 0.5f);
                else if (sprayType == SprayType.Water && current >= 0.5f)
                    colors[i].r = Mathf.Clamp(current + effect, 0.5f, 1.0f);
            }
        }
    }
}