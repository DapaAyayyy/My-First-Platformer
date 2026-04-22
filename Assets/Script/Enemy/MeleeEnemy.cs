using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameter")]
    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;

    [Header("Collider Apa gitu")]
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private float colliderDistance;
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    [Header("Audio Clip")]
    [SerializeField] private AudioClip attackSound;
    private float cooldownTimer = Mathf.Infinity;
    
    private Health playerHealth;
    private Animator anim;
    private EnemyPatrol enemyPatrol;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(PlayerInSight()){
            if(cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
                {
                    SoundManager.instance.PlaySound(attackSound);
                    cooldownTimer = 0;
                    anim.SetTrigger("meleeAttack");
                }
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider2D.bounds.size.x *range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        
        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider2D.bounds.size.x *range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.takeDamage(damage);
        }
    }
}
