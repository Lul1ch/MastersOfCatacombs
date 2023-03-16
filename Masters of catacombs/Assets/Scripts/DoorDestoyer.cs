using UnityEngine;

public class DoorDestoyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Door")) {
            Destroy(other.gameObject);
        }
    }
}
