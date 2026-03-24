using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Point")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField]private float speed;
    [SerializeField]private Transform enemy;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Time")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Animator")]
    [SerializeField] private Animator anim;
    
    private void Awake()
    {
        initScale = enemy.localScale;    
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }
    private void Update()
    {
        if (movingLeft)
        {
            if(enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                ChangeDirection();
        }
        else
        {
            if(enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                ChangeDirection();
        }
        
    }
    private void ChangeDirection()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }

    }
    private void MoveInDirection (int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        //make enemy face the right direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        //move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
