using UnityEngine;

public class EnemyFireballHolder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform enemy;

    public void Update()
    {
        transform.localScale = enemy.localScale;
    }
}
