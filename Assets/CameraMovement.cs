using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;    // Reference to the player's transform
    public float smoothing = 5f; // Smooth speed for camera movement
    public Vector2 offset;      // Offset from the player's position

    private float fixedY;       // Fixed Y position of the camera

    void Start()
    {
        // Set the fixed Y position to the camera's initial Y position
        fixedY = transform.position.y;
    }

    void FixedUpdate()
    {
        // Get the target position based on the player's X position and fixed Y position
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, fixedY + offset.y, transform.position.z);

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}
