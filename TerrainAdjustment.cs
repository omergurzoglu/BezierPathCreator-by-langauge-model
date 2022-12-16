
using UnityEngine;

public class TerrainAdjustment : MonoBehaviour
{
    // The player object
    public GameObject player;

    // The maximum slope angle that the player can walk on
    public float maxSlopeAngle = 45f;

    // The distance of the ray that is cast downward from the player
    public float rayDistance = 1f;

    // The layer mask for the terrain
    public LayerMask terrainLayerMask;

    void Update()
    {
        // Cast a ray downward from the player
        Ray ray = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit;

        // Check if the ray hits the terrain
        if (Physics.Raycast(ray, out hit, rayDistance, terrainLayerMask))
        {
            // Get the normal of the terrain at the point where the ray hit
            Vector3 normal = hit.normal;

            // Get the angle between the normal and the up vector
            float angle = Vector3.Angle(normal, Vector3.up);

            // If the angle is greater than the maximum slope angle, adjust the player's angle
            if (angle > maxSlopeAngle)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(player.transform.up, normal) * player.transform.rotation;
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}