using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    Vector2 moveDirection;

    [SerializeField] private Animator anim;

    Rigidbody2D rb;
    PlayerInput input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        input = new PlayerInput();
        input.Movement.Enable();
    }

    private void OnEnable()
    {
        input.Movement.Enable();
    }

    private void OnDisable()
    {
        input.Movement.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 moveInput = input.Movement.Movement.ReadValue<Vector2>();
        moveDirection = moveInput.x * transform.right + moveInput.y * transform.up;

        anim.SetFloat("Speed", moveDirection.normalized.magnitude / moveSpeed);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
