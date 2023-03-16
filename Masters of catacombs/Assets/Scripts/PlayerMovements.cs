using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
  
    [Header("Player data")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] private SpriteRenderer playerSprite;

    private float plSpeed = 6f;
    private float hDirection = 0f, vDirection = 0f;

    private void Update() {
        Move();
        Lunge();
        if (Input.GetKey(KeyCode.F)){
            playerTransform.localPosition = Vector3.Lerp(playerTransform.localPosition, new Vector3(0, 1f,0), 5f);
        }
    }

    private void Move() {
        hDirection = Input.GetAxisRaw("Horizontal") * plSpeed;
        vDirection = Input.GetAxisRaw("Vertical") * plSpeed;

        if (hDirection < 0) {
            playerSprite.flipX = true;
        } else if (hDirection > 0){
            playerSprite.flipX = false;
        }

        playerRb.velocity = new Vector2(hDirection, vDirection);
    }

    private int lungeImpulse = 5000;
    private void Lunge() {
        if (Input.GetKeyDown(KeyCode.X) && !lungeLocked) {
            lungeLocked = true;
            Invoke("UnlockLunge", 2f);

            hDirection = Input.GetAxisRaw("Horizontal");
            vDirection = Input.GetAxisRaw("Vertical");
            Debug.Log("Deshed");
            playerRb.AddForce(new Vector2(hDirection, vDirection) * lungeImpulse);
        }
    }

    private bool lungeLocked = false;
    private void UnlockLunge() {
        lungeLocked = false;
    }
}
