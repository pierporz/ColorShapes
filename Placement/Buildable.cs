using UnityEngine;

public class Buildable : MonoBehaviour
{
    public Vector3 placementOffset = new Vector3(0, 0.5f, 0);
    public Vector3 occupancySize = Vector3.one; // larghezza, altezza, profondità occupate
}
