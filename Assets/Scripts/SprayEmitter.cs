using UnityEngine;

public class SprayEmitter : MonoBehaviour
{
    public enum SprayType { Foam, Water }
    public SprayType sprayType;

    public float sprayRange = 10f;
    public float sprayRadius = 0.5f;
    public LayerMask hitLayers;

    public ParticleSystem sprayParticle;

    void Start()
    {
        // Başlangıçta particle sistem kapalı olsun
        if (sprayParticle != null && sprayParticle.isPlaying)
        {
            sprayParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void Update()
    {
        if (SprayerManager.Instance == null || SprayerManager.Instance.GetActiveSprayer() != transform.parent)
            return;
        bool sprayActive = Input.GetMouseButton(1);

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

        RaycastHit[] hits = Physics.SphereCastAll(origin, sprayRadius, direction, sprayRange, hitLayers);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Car"))
            {
                Debug.Log($"{sprayType} sprayed on car at: {hit.point}");

                // Gelecekte burada shader/texture değiştirme yapılacak
            }
        }

        // (Sahne gösterimi için opsiyonel)
        Debug.DrawRay(origin, direction * sprayRange, Color.green);
    }
}