using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleRangeEnemy : MonoBehaviour
{
   private enum Status {
        Chilling, Patrolling, MovingBack, Attacking
    }

    private Status enemyStatus = Status.Attacking;
    private float enemySpeed = 3f;
    private float missileSpeed = 5f;
    private Vector2 startEnemyPos;
    
    private Vector2 curFinishPos;
    private bool isFinishPosDefined;
    private bool isEnemyShooting;
    private Coroutine attackCoroutine;

    private NavMeshAgent agent;

    [SerializeField] private GameObject missileSample;

    private void Start() {
        startEnemyPos = gameObject.transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update() {
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
                isEnemyShooting = false;
                GoBackToStartPos();
                if (isOnTheStartPos()) {
                    enemyStatus = Status.Chilling;
                }
                break;
            case Status.Attacking:
                if (!isEnemyShooting) {
                    Attack();
                    isEnemyShooting = true;
                }
                KeepDistance();
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

    private void KeepDistance() {
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (Vector2.Distance(playerPos, transform.position) >= 15f) {
            agent.SetDestination(playerPos);
            //transform.position = Vector2.MoveTowards(transform.position, playerPos, enemySpeed * Time.deltaTime);
        } else if (Vector2.Distance(playerPos, transform.position) <= 10f) {
            //agent.SetDestination(2*(new Vector2(transform.position.x - playerPos.x, transform.position.y - playerPos.y)));
            //transform.position = Vector2.MoveTowards(transform.position, 2*(new Vector2(transform.position.x - playerPos.x, transform.position.y - playerPos.y)), enemySpeed * Time.deltaTime);
        }
    }

    private void Attack() {
        attackCoroutine = StartCoroutine(ShootTheMissile());
        //Tut on atakuet
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag != "Player") {
            if  (enemyStatus != Status.Attacking) {
                enemyStatus = Status.MovingBack;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "PlayerAura") {
            enemyStatus = Status.Attacking;
        } if (other.CompareTag("DoorTrigger")) {
            //enemyStatus = Status.MovingBack;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.name == "PlayerAura") {
            //enemyStatus = Status.Attacking;
        }
    }

    public IEnumerator ShootTheMissile() {
        while(enemyStatus == Status.Attacking) {
            Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            GameObject newMissile = Instantiate(missileSample, gameObject.transform.position, Quaternion.identity);
            newMissile.GetComponent<Missile>().initTheMissile(missileSpeed, playerPos); 

            yield return new WaitForSeconds(3f);
        }

        //StopCoroutine(attackCoroutine);
    }
}
