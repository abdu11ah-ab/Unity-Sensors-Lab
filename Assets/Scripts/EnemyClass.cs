using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    // Public properties (to set in Inspector)
    public Material defaultMat;
    public Transform Player; // The player object

    // Private properties for sight logic
    private Vector3 targetDirection;
    private Quaternion targetRotation;
    private float targetAngle;
    private float turnSpeed = 5.0f;
    private float fov = 30.0f;
    private float viewDistance = 10.0f;

    // Update is called once per frame
    void Update()
    {
        //Determine target direction (player pos - enemy pos)
        targetDirection = Player.position - transform.position;

        // Determine rotation to face the player
        targetRotation = Quaternion.LookRotation(targetDirection);

        // Slowly rotate (Slerp) towards the player
        //SKILL DEMO: 13. Use spherical linear interpolation to smoothly turn a game object over time.
        this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // Determine if player is in the Field of View (FOV)
        // Get the angle between enemy's forward and direction to player
        targetAngle = Vector3.Angle(targetDirection, transform.forward);

        // 5. Cast a ray to see what's in front
        RaycastHit hit;

        // Cast a ray from enemy, in direction of player, for viewDistance
        // We must check BOTH the angle and if the raycast hits something
        if (targetAngle < fov * 0.5f && Physics.Raycast(transform.position, targetDirection, out hit, viewDistance))
        {
            // A. If Yes: Set enemy material to the material of the object it hit
            GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
        }
        else
        {
            // B. If No (out of range, out of FOV, or hit nothing): Revert to default
            GetComponent<Renderer>().material = defaultMat;
        }
    }

    // Draws all the debug guides

    //SKILL DEMO: 16. Draw a Gizmo
    void OnDrawGizmos()
    {
        // 1. Draw red line from enemy to player
        if (Player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Player.position);
        }

        // 2. Draw the Field of View cone
        Gizmos.color = Color.green;

        // Calculate the left FOV ray
        Quaternion leftRayRotation = Quaternion.Euler(0, -fov * 0.5f, 0);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 leftRayPt = transform.position + (leftRayDirection * viewDistance);
        Gizmos.DrawLine(transform.position, leftRayPt);

        // Calculate the right FOV ray
        Quaternion rightRayRotation = Quaternion.Euler(0, fov * 0.5f, 0);
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Vector3 rightRayPt = transform.position + (rightRayDirection * viewDistance);
        Gizmos.DrawLine(transform.position, rightRayPt);

        // Connect the ends of the rays to make a triangle
        Gizmos.DrawLine(leftRayPt, rightRayPt);
    }
}