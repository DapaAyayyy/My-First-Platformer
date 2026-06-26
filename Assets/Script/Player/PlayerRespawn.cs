using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    public void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {

        //Check apakah checkpoint masih tersedia(belum diambil)
        if(currentCheckpoint == null)
        {
            //Menampilkan UI Game Over
            uiManager.GameOver();

            return; //disini berarti kita ga akan melakukan respawn
        }


        transform.position = currentCheckpoint.position; //Moving player to the checkpoint when respawn
        playerHealth.Respawn(); //Restore Player health and reset animation

        //Move camera to checkpoint room (**for this to work the checkpoint object has to be placed
        //as a child of the room)
        Camera.main.GetComponent<Cameracontroller>().moveToNewRoom(currentCheckpoint.parent);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; //Mengganti checkpoint ke letak collision dengan tag checkpoint terjadi
            SoundManager.instance.PlaySound(checkpointSound); //Memainkan suara checkpoint
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear"); //Trigger animasi appear checkpoint

        }
    }
}
