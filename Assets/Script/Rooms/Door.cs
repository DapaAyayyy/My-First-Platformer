using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private Cameracontroller cam;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            {
                cam.moveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);
            }
            else
            {
                cam.moveToNewRoom(previousRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }
}

/*using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private Cameracontroller cam;

    // 1. SAAT PEMAIN MENYENTUH PINTU (MASUK)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Apapun yang terjadi, NYALAKAN DUA-DUANYA dulu biar aman.
            // Biar trap di belakang gak ilang, dan trap di depan siap menyambut.
            if (previousRoom.GetComponent<Room>() != null)
                previousRoom.GetComponent<Room>().ActivateRoom(true);

            if (nextRoom.GetComponent<Room>() != null)
                nextRoom.GetComponent<Room>().ActivateRoom(true);
        }
    }

    // 2. SAAT PEMAIN LEPAS DARI PINTU (KELUAR)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Nah, sekarang kita cek: Pemain keluarnya ke arah mana?
            
            // Cek posisi X pemain dibandingkan posisi X pintu
            if (collision.transform.position.x < transform.position.x)
            {
                // BERARTI: Pemain keluar ke arah KIRI (Balik ke Previous Room)
                cam.moveToNewRoom(previousRoom);
                
                // Matikan ruangan Kanan (Next Room) karena kita gak jadi ke sana
                nextRoom.GetComponent<Room>().ActivateRoom(false);
            }
            else
            {
                // BERARTI: Pemain keluar ke arah KANAN (Masuk ke Next Room)
                cam.moveToNewRoom(nextRoom);
                
                // Matikan ruangan Kiri (Previous Room) karena kita sudah fix pindah
                previousRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }
}*/
