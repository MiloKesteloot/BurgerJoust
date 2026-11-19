using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputActionAsset InputAction;

    private InputAction moveAction;

    private float moveSpeed = 20f;
    private Vector2 moveAmt;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveAmt = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }   

    private void Move()
    {
        rb.MovePosition(rb.position + new Vector2(moveAmt.x * moveSpeed * Time.fixedDeltaTime, 0));
    }
}
