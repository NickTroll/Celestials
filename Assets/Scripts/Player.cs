using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float jumpHeight;
    [SerializeField] float dashAmount;
    [SerializeField] float dashTime;
    [SerializeField] float wallFriction;
    [SerializeField] Vector2 wallJumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] StartMenu menu;
    [SerializeField] AudioSource dashSound;
    float xScale;

    Rigidbody2D rb;
    Vector2 velocity;
    private Vector2 moveDirection;
    private bool isGrounded;
    private bool isDashing;
    private bool onWall;
    private bool isFlipped;
    private bool facingLeft;
    private bool canDash;



    // Start is called before the first frame update
    void Start()
    {
        Manager.Init(this, menu);
        Manager.SetStartControls();
        //Manager.SetGameControls();
        rb = GetComponent<Rigidbody2D>();
        isDashing = false;
        isFlipped = false;
        xScale = transform.localScale.x;

    }

    // Update is called once per frame
    void Update()
    {
        WallDirection();

        if (!isDashing)
        {
            Vector2 targetVelocity = new Vector2(moveDirection.x, 0.0f) * speed;
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetVelocity.x, acceleration * Time.deltaTime), rb.velocity.y);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), rb.velocity.y);

        }

        if (IsGrounded())
        {
            canDash = true;
        }

        if (onWall && !IsGrounded())
        {
            //Debug.Log("less gravity");
            rb.drag = wallFriction;
        }
        else
        {
            rb.drag = 0;
        }
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce((Vector2.up * jumpHeight), ForceMode2D.Impulse);
        }
        else if (isFlipped && UpsideDownGrounded())
        {
            rb.AddForce((Vector2.down * jumpHeight), ForceMode2D.Impulse);
        }
        else if (onWall && !IsGrounded())
        {
            if (!isFlipped)
            {
                if (facingLeft)
                {
                    StartCoroutine(IsDashing());
                    rb.drag = 0;
                    Vector2 jumpDirection = new Vector2(wallJumpForce.x * 1, wallJumpForce.y);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(jumpDirection, ForceMode2D.Impulse);
                }
                else
                {
                    StartCoroutine(IsDashing());
                    rb.drag = 0;
                    Vector2 jumpDirection = new Vector2(wallJumpForce.x * -1, wallJumpForce.y);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(jumpDirection, ForceMode2D.Impulse);
                }
            }
            else
            {
                if (facingLeft)
                {
                    StartCoroutine(IsDashing());
                    rb.drag = 0;
                    Vector2 jumpDirection = new Vector2(wallJumpForce.x * 1, -wallJumpForce.y);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(jumpDirection, ForceMode2D.Impulse);
                }
                else
                {
                    StartCoroutine(IsDashing());
                    rb.drag = 0;
                    Vector2 jumpDirection = new Vector2(wallJumpForce.x * -1, -wallJumpForce.y);
                    rb.velocity = Vector2.zero;
                    rb.AddForce(jumpDirection, ForceMode2D.Impulse);
                }
            }
        }
    }

    public void SetMovementDirection(Vector2 currentDirection)
    {
        moveDirection = currentDirection;


    }

    public void SetLeft()
    {
        transform.localScale = new Vector2(-xScale, transform.localScale.y);
    }
    public void SetRight()
    {
        transform.localScale = new Vector2(xScale, transform.localScale.y);
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        if (hit.collider != null)
        {
            //Debug.Log("Grounded");
            return true;
        }
        else
        {
            //Debug.Log("Not Grounded");
            return false;
        }
    }

    private void WallDirection()
    {
        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, 1.1f, wallLayer);
        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, 1.1f, wallLayer);
        if (left.collider != null)
        {
            //Debug.Log("facing left");
            facingLeft = true;
        }
        else if (right.collider != null)
        {
            //false means right
            facingLeft = false;
        }



    }

    private bool UpsideDownGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1.1f, groundLayer);
        if (hit.collider != null)
        {
            //Debug.Log("Grounded");
            return true;
        }
        else
        {
            //Debug.Log("Not Grounded");
            return false;
        }
    }

    public void Dash()
    {
        if (canDash)
        {
            StartCoroutine(IsDashing());
            rb.AddForce(moveDirection * dashAmount, ForceMode2D.Impulse);
            dashSound.Play();
            canDash = false;
        }
    }

    public IEnumerator IsDashing()
    {
        isDashing = true;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;

        yield return null;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision");
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("on walll");
            onWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //Debug.Log("not on wall");
            onWall = false;
        }
    }


    public void Flip()
    {
        isFlipped = !isFlipped;
        rb.gravityScale = -rb.gravityScale;
        transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
    }

}
