using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    private Animator anim;
    public float currentHealth {get; private set;}
    private bool dead;

    [Header("Iframes")]
    [SerializeField]private float iFramesDuration;
    [SerializeField]private int numberOfFlashes;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void takeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage , 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                //player mechanism\
                if(GetComponent<PlayerMovement>() != null)
                    GetComponent<PlayerMovement>().enabled = false;

                //Enemy mechanism
                if( GetComponentInParent<EnemyPatrol>() != null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;
                if( GetComponent<MeleeEnemy>() != null)
                    GetComponent<MeleeEnemy>().enabled = false;

                foreach (Behaviour component in components)
                    component.enabled = false;
                
                dead = true;
            }
        }


    }
    //Mekanisme Penambah darah
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value , 0, startingHealth);

    }

    //Invincible function
    private IEnumerator Invulnerability()//Invincible function
    {
    //mekanisme biar invincible
    Physics2D.IgnoreLayerCollision(10,11,true);
    //waktu sprite karakter blink
    for (int i = 0; i < numberOfFlashes; i++)
    {
        spriteRenderer.color = new Color(1, 0, 0 , 0.5f);
        yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

    }
    //Buat collision bisa terjadi lagi
    Physics2D.IgnoreLayerCollision(10,11,false);
    }
        public void Deactivate()
    {
        gameObject.SetActive(false);   
    }
}


