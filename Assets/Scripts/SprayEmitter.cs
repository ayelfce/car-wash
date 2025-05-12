using UnityEngine;

public class SprayEmitter : MonoBehaviour
{
    public enum SprayType { Foam, Water }
    [SerializeField] private SprayType sprayType;

    [SerializeField] private float sprayRange = 10f;
    [SerializeField] private float sprayRadius = 0.2f;
    [SerializeField] private LayerMask hitLayers;

    [SerializeField] private ParticleSystem sprayParticle;

    void Start()
    {
        if (sprayParticle != null && sprayParticle.isPlaying)
        {
            sprayParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void Update()
    {
        if (SprayerManager.Instance == null || SprayerManager.Instance.GetActiveSprayer() != transform.parent)
            return;

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
        Transform rayOrigin = GetRayOrigin();

        if (rayOrigin == null)
        {
            Debug.LogWarning("RayOrigin child objesi bulunamadı!");
            return;
        }

        Vector3 origin = rayOrigin.position;
        Vector3 direction = rayOrigin.forward + (-rayOrigin.up * 0.3f);
        direction.Normalize();

        RaycastHit[] hits = Physics.SphereCastAll(origin, sprayRadius, direction, sprayRange, hitLayers);

        int maxPaints = 5;
        int painted = 0;

        foreach (RaycastHit hit in hits)
        {
            if (painted >= maxPaints) break;

            if (hit.collider.CompareTag("Car"))
            {
                CarPaintManager paintManager = hit.collider.GetComponent<CarPaintManager>();
                if (paintManager != null)
                {
                    ApplyVertexBrush(paintManager, hit.point);
                    painted++;
                }
            }
        }
    }

    void ApplyVertexBrush(CarPaintManager paintManager, Vector3 worldHitPoint)
    {
        Mesh mesh = paintManager.GetMesh();
        if (mesh == null) return;

        Vector3[] vertices = mesh.vertices;
        Color[] colors = mesh.colors;

        Transform tf = paintManager.transform;
        float targetValue = sprayType == SprayType.Foam ? 0.4f : 1.0f;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = tf.TransformPoint(vertices[i]);
            float dist = Vector3.Distance(worldHitPoint, worldPos);

            if (dist < sprayRadius)
            {
                float current = colors[i].r;
                colors[i].r = Mathf.Clamp(current + 0.05f, 0f, targetValue);
            }
        }

        mesh.colors = colors;
    }

    private Transform GetRayOrigin()
    {
        return transform.Find("RayOrigin");
    }
}