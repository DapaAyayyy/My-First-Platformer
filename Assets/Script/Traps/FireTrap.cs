using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private float damage;

    [Header("Firetrap Timer")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    [Header("SFX")]
    [SerializeField] private AudioClip fireTrapSound;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool active;
    private bool triggered;
    private Health player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
                StartCoroutine(ActivateFireTrap());
            
            player = collision.GetComponent<Health>();
        }
    }
    private void OnTriggerExit2D()
    {
        player = null;
    }

    private void FixedUpdate()
    {
        if (active && player != null)
        {   
            player.takeDamage(damage);
            player = null;
        }
    }

    private IEnumerator ActivateFireTrap()
    {
        triggered = true;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(fireTrapSound);
        spriteRenderer.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);

    }
}
