using UnityEngine;

public class SprayerManager : MonoBehaviour
{
    public static SprayerManager Instance;

    public Transform car; // Arabayı buraya assign et
    public float moveSpeed = 5f;
    public float followDistance = 2f;

    private Transform activeSprayer;
    private Camera mainCam;

    private float mouseX;
    private float mouseY;

    private void Awake()
    {
        Instance = this;
        mainCam = Camera.main;
    }

    public void SetActiveSprayer(Transform sprayer)
    {
        activeSprayer = sprayer;
    }

    void Update()
    {
        HandleSelectionInput();

        if (activeSprayer != null)
        {
            HandleMovement();
        }
    }

    private void HandleSelectionInput()
    {
        // SEÇİMİ KALDIR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activeSprayer = null;
            return;
        }

        // SEÇME
        if (Input.GetMouseButtonDown(0)) // Sol tık
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Sprayer"))
                {
                    SetActiveSprayer(hit.transform);
                }
            }
        }
    }

    private void HandleMovement()
    {
        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        // Sağ/sol ve yukarı/aşağı hareket
        Vector3 move = new Vector3(deltaX, deltaY, 0f) * moveSpeed * Time.deltaTime;
        activeSprayer.Translate(move, Space.World);

        // Arabaya sabit mesafe (Z ekseni ayarlanıyor)
        Vector3 dirToCar = (car.position - activeSprayer.position).normalized;
        Vector3 targetPos = car.position - dirToCar * followDistance;

        // Z'yi yeniden ayarla ama X ve Y'yi koru
        activeSprayer.position = new Vector3(
            activeSprayer.position.x,
            activeSprayer.position.y,
            targetPos.z
        );

        // Arabaya dönsün
        activeSprayer.LookAt(car);
    }

    public Transform GetActiveSprayer()
    {
        return activeSprayer;
    }
}