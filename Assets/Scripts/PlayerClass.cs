using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    // Public properties for speed
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 200.0f;

    // Update is called once per frame
    void Update()
    {
        // Get "Vertical" input (Up/Down arrows)
        float moveInput = Input.GetAxis("Vertical");
        // Move the player forward/backward
        this.transform.Translate(0, 0, moveInput * movementSpeed * Time.deltaTime);

        // Get "Horizontal" input (Left/Right arrows)
        float rotateInput = Input.GetAxis("Horizontal");
        // Rotate the player left/right
        this.transform.Rotate(0, rotateInput * rotationSpeed * Time.deltaTime, 0);
    }

    // Called when this object's trigger collides with another collider
    void OnTriggerEnter(Collider col)
    {
        // Check if the object we hit is named "Enemy"
        if (col.gameObject.name == "Enemy")
        {
            // Double the enemy's current size
            col.gameObject.transform.localScale *= 2.0f;
        }
    }

    // Draws the green "forward" line for the player
    void OnDrawGizmos()
    {
        // Draw a green ray from the player's position, in its forward direction
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 2); // 2 units long
    }
}