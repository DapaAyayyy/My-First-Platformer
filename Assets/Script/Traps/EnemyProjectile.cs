using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;
    private bool hit;
    public void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }
public void ActivateProjectile()
    {
        // 1. NYALAKAN DULU OBJEKNYA!
        // Ini akan memancing fungsi Awake() jalan dan mengisi variabel 'coll'
        gameObject.SetActive(true); 

        // 2. Karena Awake() sudah jalan, sekarang 'coll' aman untuk dipakai
        coll.enabled = true;
        hit = false;
        lifetime = 0;
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed,0,0);

        lifetime += Time.deltaTime;
        if (lifetime  > resetTime)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        coll.enabled = false;
        hit = true;
        base.OnTriggerEnter2D(collision);

        if(anim != null)
            anim.SetTrigger("explode");
        else
            gameObject.SetActive(false);
    }
    private void Deactivate()
    {
            gameObject.SetActive(false);
    }
}
