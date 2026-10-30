using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public float speed = 10f; // Give it a base speed value
    public float maxSpeed = 10f; // NEW: Maximum speed limit

    // NEW: Jump configurations
    [Header("Jumping settings")]
    public float jumpForce = 5.0f;
    private bool isGrounded;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    [Header("Timer Reference")]
    public TimerController timerController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // NEW: Triggered when the Jump action button (e.g., Spacebar) is pressed
    void OnJump()
    {
        if (isGrounded)
        {
            // Applies an immediate upward physics impulse force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            if (timerController != null)
            {
                timerController.StopTimer();
            }
        }
    }

    void FixedUpdate()
    {
        // 1. Apply movement force
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        // 2. Clamp horizontal velocity (X and Z) so jumping (Y) isn't affected
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 clampedVelocity = horizontalVelocity.normalized * maxSpeed;
            // Apply clamped horizontal velocity while preserving the vertical jump velocity
            rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    // UPDATED: Standard physical collisions handle death conditions AND ground detection
    private void OnCollisionEnter(Collision collision)
    {
        // 1. Check for ground tag to reset jumping capability
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // 2. Check for hazard/enemy tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (timerController != null)
            {
                timerController.StopTimer();
            }
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose! (Press R to Restart)";
        }
    }
}
