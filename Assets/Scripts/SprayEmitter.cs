using UnityEngine;

public class SprayEmitter : MonoBehaviour
{
    public enum SprayType { Foam, Water }
    [SerializeField] private SprayType sprayType;

    [SerializeField] private float sprayRange = 10f;
    [SerializeField] private float sprayRadius = 0.2f;
    [SerializeField] private LayerMask hitLayers;

    [SerializeField] private ParticleSystem sprayParticle;
    [SerializeField] private SprayBrushPainter brushPainter;

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
                Vector2 uv = hit.textureCoord;
                brushPainter?.PaintAt(uv, sprayType);
                painted++;
            }
        }

        Debug.DrawRay(origin, direction * sprayRange, Color.green);
    }

    private Transform GetRayOrigin()
    {
        return transform.Find("RayOrigin");
    }
}