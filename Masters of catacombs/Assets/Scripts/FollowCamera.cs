using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    private void Update() {
        MoveCamera();
    }

    private void MoveCamera() {
        gameObject.transform.position = new Vector3 (playerTransform.position.x, playerTransform.position.y, -10);
    }
}
