using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 10;

    [Space(1)]
    [SerializeField] float accel;
    [SerializeField] float deAccel;

    [Space(1)]
    [Header("Dash")]
    [SerializeField] float dashPower = 15;
    [SerializeField] float dashCooldown = 1f;

    [Space(1)]
    [Header("Jump")]
    [SerializeField] float jumpPower = 15;
    [SerializeField] float jumpCooldown = 0.5f;
    [SerializeField] int totalJumps = 2; 
    [SerializeField] LayerMask groundLayer;


    Rigidbody2D rb;

    float dirX = 1;

    int currentJump = 0;

    float lastDashTime = -0;
    float lastLandTime = -0;

    public bool isGrounded { get; private set; }
    public float InputX { get; private set; }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputX = Input.GetAxis("Horizontal");

        if (InputX != 0)
            dirX = InputX > 0 ? 1 : -1;

        isGrounded = Physics2D.Raycast(transform.position, Vector3.down, 1.5f, groundLayer);

        HandleJumping();
        ApplyDash();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;

            InputX = 0;

            float targetSpeed = dirX * dashPower;

            rb.linearVelocityX += targetSpeed;

            lastDashTime = Time.time; // reset cooldown
        }
    }

    void ApplyMovement()
    {
        float targetSpeed = InputX * speed;
        float diff = targetSpeed - rb.linearVelocityX;

        float targetAccel = Mathf.Abs(targetSpeed) > 0.1f ? accel : deAccel;

        float movement = diff * targetAccel * Time.deltaTime;

        rb.linearVelocityX += movement;
    }

    private void HandleJumping()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;


        if (isGrounded)
        {
            lastLandTime += Time.deltaTime;
            currentJump = 0;
        }


        if (isGrounded && lastLandTime >= jumpCooldown)
        {
            currentJump++;

            rb.linearVelocityY = jumpPower;

            lastLandTime = 0;

            return;
        }


        if (currentJump < totalJumps)
        {
            rb.linearVelocityY = jumpPower;

            lastLandTime = 0;

            currentJump++;

            return;
        }
    }
}
