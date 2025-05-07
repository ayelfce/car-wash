using UnityEngine;

public class SprayerManager : MonoBehaviour
{
    public static SprayerManager Instance;
    [SerializeField] private Transform car;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float followDistance = 2f;

    [SerializeField] private Transform activeSprayer;
    [SerializeField] private Camera mainCam;
    
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
        // seçimi kaldır
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activeSprayer = null;
            return;
        }

        // seç
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

        // sağ/sol ve yukarı/aşağı hareket
        Vector3 move = new Vector3(deltaX, deltaY, 0f) * moveSpeed * Time.deltaTime;
        activeSprayer.Translate(move, Space.World);

        // arabaya sabit mesafe
        Vector3 dirToCar = (car.position - activeSprayer.position).normalized;
        Vector3 targetPos = car.position - dirToCar * followDistance;

        activeSprayer.position = new Vector3(
            activeSprayer.position.x,
            activeSprayer.position.y,
            targetPos.z
        );

        activeSprayer.LookAt(car);
    }

    public Transform GetActiveSprayer()
    {
        return activeSprayer;
    }
}