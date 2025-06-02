using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public int gridSize = 100;
    public float cellSize = 1f;
    public Color gridColor = Color.green;
    public Transform followTarget; // es. il player


    public void GenerateGrid(Vector3 center)
    {
        // Pulisce vecchie linee
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        float halfGrid = (gridSize * cellSize) / 2f;
        Vector3 origin = new Vector3(
            Mathf.Floor(center.x / cellSize) * cellSize - halfGrid,
            0,
            Mathf.Floor(center.z / cellSize) * cellSize - halfGrid
        );

        for (int i = 0; i <= gridSize; i++)
        {
            float pos = i * cellSize + 0.5f;
            CreateLine(new Vector3(origin.x + pos, 0, origin.z), new Vector3(origin.x + pos, 0, origin.z + gridSize * cellSize));
            CreateLine(new Vector3(origin.x, 0, origin.z + pos), new Vector3(origin.x + gridSize * cellSize, 0, origin.z + pos));
        }
    }


    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = transform;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = gridColor;
        lr.endColor = gridColor;
        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
