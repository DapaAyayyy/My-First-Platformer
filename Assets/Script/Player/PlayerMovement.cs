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
        if (wallJumpCooldown > 0.2f)
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

            if (onWall() && !isGrounded())
            {
                 rb.gravityScale = 0;
                 rb.velocity = Vector2.zero;
            }
            else
            {
                rb.gravityScale = 3;
            }
            if (Input.GetKeyDown(KeyCode.Space) ) // Pakai GetKeyDown
                {
                    Jump();
                }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }




    private void Jump()
    {
        if (isGrounded()){
            SoundManager.instance.PlaySound(jumpSound);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("jump");}
        else if(onWall() && !isGrounded())
        {   if (horizontalInput == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10 , 6);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y,transform.localScale.z);
            }
            else 
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3 , 6);
            
            wallJumpCooldown = 0;
        }
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