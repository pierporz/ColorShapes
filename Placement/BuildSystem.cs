using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public GameObject ghostPrefab;
    public GameObject realBlockPrefab;
    public GameObject bigGhostPrefab;
    public GameObject bigRealBlockPrefab;

    public Material ghostMaterial;
    public GameObject gridVisual;
    public GridRenderer gridRenderer;
    public Transform playerTransform;
    public LayerMask groundLayer;
    public float maxBuildDistance = 10f;

    private GameObject ghostInstance;
    private GameObject currentGhostPrefab;
    private GameObject currentRealPrefab;

    private bool buildMode = false;
    private bool isOccupied = false;
    private Vector3 lastValidPosition;
    private bool hasValidPosition = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnterBuildMode(ghostPrefab, realBlockPrefab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnterBuildMode(bigGhostPrefab, bigRealBlockPrefab);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitBuildMode();
        }

        if (buildMode && ghostInstance != null)
        {
            UpdateGhostPosition();

            if (Input.GetKeyDown(KeyCode.E) && hasValidPosition && !isOccupied)
            {
                Instantiate(currentRealPrefab, lastValidPosition, Quaternion.identity);
            }
        }
    }

    void EnterBuildMode(GameObject ghost, GameObject real)
    {
        if (ghostInstance != null)
        {
            Destroy(ghostInstance);
        }

        currentGhostPrefab = ghost;
        currentRealPrefab = real;

        ghostInstance = Instantiate(currentGhostPrefab);
        ghostInstance.GetComponent<Renderer>().material = ghostMaterial;

        gridRenderer.GenerateGrid(playerTransform.position);
        gridVisual.SetActive(true);
        buildMode = true;
        hasValidPosition = false;
    }

    void ExitBuildMode()
    {
        if (ghostInstance != null)
        {
            Destroy(ghostInstance);
        }

        gridVisual.SetActive(false);
        buildMode = false;
    }

    void UpdateGhostPosition()
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // piano a Y = 0
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = (hitPoint - playerTransform.position).normalized;
            float rawDistance = Vector3.Distance(playerTransform.position, hitPoint);

            float clampedDistance = Mathf.Min(rawDistance, maxBuildDistance);
            Vector3 limitedPoint = playerTransform.position + direction * clampedDistance;

            // Snap alla griglia
            Vector3 snapped = new Vector3(
                Mathf.Round(limitedPoint.x),
                0f,
                Mathf.Round(limitedPoint.z)
            );

            Buildable buildable = ghostInstance.GetComponent<Buildable>();
            Vector3 offset = buildable != null ? buildable.placementOffset : Vector3.zero;
            Vector3 size = buildable != null ? buildable.occupancySize * 0.5f : Vector3.one * 0.45f;

            Vector3 finalPos = snapped + offset;
            ghostInstance.transform.position = finalPos;

            isOccupied = Physics.CheckBox(finalPos, size);
            Color color = isOccupied ? Color.red : new Color(0.6f, 0.6f, 0.6f, 0.3f);
            ghostInstance.GetComponent<Renderer>().material.color = color;

            lastValidPosition = finalPos;
            hasValidPosition = true;
        }
    }



}
