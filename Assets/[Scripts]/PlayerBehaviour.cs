using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public Transform groundOrigin;

    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckIfGrounded();
        Move();
    }

    private void Move()
    {
        if (isGrounded)
        {
            // Keyboard / mouse Input
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");
            float jump = Input.GetAxisRaw("Jump");

            // Check for flip
            if (x != 0)
            {
                x = FlipAnimation(x);
            }

            Vector2 worldTouch = new Vector2();

            // Touch Input
            foreach (var touch in Input.touches)
            {
                worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            var horizontalMoveForce = x * horizontalForce;
            var jumpMoveForce = jump * verticalForce;

            float mass = rigidBody.mass * rigidBody.gravityScale;

            rigidBody.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce));
            rigidBody.velocity *= 0.99f;
        }
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

    private float FlipAnimation(float x)
    {
        // Scale x-axis based on direction
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);

        return x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
    }
}
