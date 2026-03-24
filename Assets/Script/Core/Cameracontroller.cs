using UnityEngine;

public class Cameracontroller : MonoBehaviour
{   //room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;
    //player camera
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;
    private void Update()
    {
        //room camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y,transform.position.z), ref velocity,  speed);

        //player camera
        transform.position = new Vector3(player.position.x+lookAhead,transform.position.y,transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x),  Time.deltaTime *cameraSpeed);}
    public void moveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
