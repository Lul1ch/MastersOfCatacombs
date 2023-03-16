using UnityEngine;

public class SimpleEnemyMelee : MonoBehaviour
{
    private enum Status {
        Chilling, Patrolling, MovingBack, Attacking
    }

    private Status enemyStatus;
    private float enemySpeed = 2f;
    private float primalEnemySpeed = 2f;
    private Vector2 startEnemyPos;
    private int aggressionMultiplier = 2;
    
    private Vector2 curFinishPos;
    private bool isFinishPosDefined;

    private void Start() {
        startEnemyPos = gameObject.transform.position;
    }

    private void Update() {
        //Debug.Log(enemyStatus);
        switch (enemyStatus)
        {
            case Status.Chilling:
                if (!isFinishPosDefined) {
                    Invoke("GoPatrol", 5f);
                    isFinishPosDefined = true;
                }
                break;
            case Status.Patrolling:
                Patrol();
                break;
            case Status.MovingBack:
                GoBackToStartPos();
                if (isOnTheStartPos()) {
                    enemyStatus = Status.Chilling;
                }
                break;
            case Status.Attacking:
                Attack();
                break;
            default:
                break;
        }
    }

    private void GoPatrol() {
        float randX = Random.Range(startEnemyPos.x - 15f, startEnemyPos.x + 15f);
        float randY = Random.Range(startEnemyPos.y - 11f, startEnemyPos.y + 11f);

        Vector2 finishPos = new Vector2(randX, randY);

        curFinishPos = finishPos;
        enemyStatus = Status.Patrolling;
    }

    private void Patrol() {
        Vector2 curEnemyPos = gameObject.transform.position;
        gameObject.transform.position = Vector2.MoveTowards(curEnemyPos, curFinishPos, enemySpeed * Time.deltaTime);
        if (Mathf.Abs(Vector2.Distance(curEnemyPos, curFinishPos)) < 0.05f) {
            enemyStatus = Status.MovingBack;
            GoBackToStartPos();
        }
    }

    private void GoBackToStartPos() {
        Vector2 curEnemyPos = gameObject.transform.position;
        gameObject.transform.position = Vector2.MoveTowards(curEnemyPos, startEnemyPos, enemySpeed * Time.deltaTime);
        isFinishPosDefined = false;
    }

    private bool isOnTheStartPos() {
        Vector2 curEnemyPos = gameObject.transform.position;
        return (Mathf.Abs(Vector2.Distance(curEnemyPos, startEnemyPos)) < 0.05f);
        }

    private void Attack() {
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, playerPos, enemySpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag != "Player") {
            enemyStatus = Status.MovingBack;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "PlayerAura") {
            enemyStatus = Status.Attacking;
            enemySpeed = primalEnemySpeed * aggressionMultiplier;
        } if (other.CompareTag("DoorTrigger")) {
            enemyStatus = Status.MovingBack;
            enemySpeed = primalEnemySpeed;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.name == "PlayerAura") {
            //enemyStatus = Status.Attacking;
        }
    }

}
