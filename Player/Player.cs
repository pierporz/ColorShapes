using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement movement;

    void Update()
    {
        movement?.HandleMovement();
    }
}
