using UnityEngine;

public class SprayEmitter : MonoBehaviour
{
    public enum SprayType { Foam, Water }
    public SprayType sprayType;

    public float sprayRange = 10f;
    public float sprayRadius = 0.2f;
    public LayerMask hitLayers;

    public ParticleSystem sprayParticle;
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
    Vector3 origin = transform.position;
    Vector3 direction = transform.forward;

    // Bu çizim, etki alanını sahnede görsel olarak gösterecek
    Debug.DrawRay(origin, direction * sprayRange, Color.green, 1f);

    // Çember (spray yarıçapı) gösterimi
    for (int i = 0; i < 360; i += 15)
    {
        float rad = i * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * sprayRadius;
        Vector3 point = origin + transform.TransformDirection(offset);
        Debug.DrawLine(origin, point, Color.red, 0.5f);
    }

    RaycastHit[] hits = Physics.SphereCastAll(origin, sprayRadius, direction, sprayRange, hitLayers);

    Debug.Log($"Spray hit {hits.Length} objects");

    int maxPaints = 5;
    int painted = 0;

    foreach (RaycastHit hit in hits)
    {
        if (painted >= maxPaints) break;

        if (hit.collider.CompareTag("Car"))
        {
            Debug.Log($"Hit point: {hit.point} | UV: {hit.textureCoord}");

            Vector2 uv = hit.textureCoord;
            brushPainter?.PaintAt(uv, sprayType);
            painted++;
        }
    }
}
}