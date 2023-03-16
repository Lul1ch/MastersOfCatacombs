using UnityEngine;

public class Missile : MonoBehaviour
{
    private Vector2 targetPossition;
    private float missileSpeed;
    private bool isMissileCreated;
    private float lifeTime = 5f;

    [SerializeField] private Rigidbody2D missileRb;

    public void initTheMissile (float missileSpeed, Vector2 targetPossition) {
        this.missileSpeed = missileSpeed;
        this.targetPossition = targetPossition;

        missileRb.velocity = new Vector2(targetPossition.x - transform.position.x, targetPossition.y - transform.position.y).normalized * missileSpeed;
        Invoke("DestroyMissile", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player") && !other.CompareTag("Enemy") && other.gameObject.name != "PlayerAura") {
            Destroy(gameObject);
        }
    }

    private void DestroyMissile() {
        Destroy(gameObject);
    }
}
