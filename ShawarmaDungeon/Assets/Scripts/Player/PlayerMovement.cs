using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    Vector2 moveDirection;

    Rigidbody2D rb;
    PlayerInput input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        input = new PlayerInput();
        input.Movement.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = input.Movement.Movement.ReadValue<Vector2>();
        moveDirection = moveInput.x * transform.right + moveInput.y * transform.up;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
