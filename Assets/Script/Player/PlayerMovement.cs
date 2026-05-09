using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed; //Kecepatan gerak
    [SerializeField] private float jumpForce; //Kekuatan Jump
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    private float wallJumpCooldown;
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private float horizontalInput;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Multiple Jump")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;
    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        // Input dibaca di Update untuk responsivitas
        horizontalInput = Input.GetAxis("Horizontal");

        // Membalik karakter
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);



        // Mengatur Animator
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        
        //WallJump logic
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();

        if(Input.GetKeyUp(KeyCode.Space)&& rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);

        if (onWall())
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = 7;
            rb.velocity = new Vector2(horizontalInput*speed, rb.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Mereset coyote counter ketika kita berada di tanah
                jumpCounter = extraJumps; //Mereset jump counter ketika kita berada di tanah
            }
            else
            {
                coyoteCounter -= Time.deltaTime; //Mengurangi Coyote counter ketika kita tidak berada di tanah/ground
            }
        }
    }




    private void Jump()
    {
         //if coyote counter = 0, jump counter = 0 dan kita berada di dinding jangan lakukan apapun
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return; 

        SoundManager.instance.PlaySound(jumpSound);
        if (onWall())
        {
            wallJump();
        }
        else{
            if (isGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else
            {
                if(coyoteCounter > 0)
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                else
                {
                    if (jumpCounter > 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                        jumpCounter--;
                    }
                }    
            }
            coyoteCounter = 0;
        }
    }
    
    private void wallJump()
    {
        rb.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit2D.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit2D.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}