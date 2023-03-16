using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public bool spawned = false;
    private int roomBorderNumber = 100;

    [SerializeField] private GameObject TopBorderRoom;
    [SerializeField] private GameObject BottomBorderRoom;
    [SerializeField] private GameObject LeftBorderRoom;
    [SerializeField] private GameObject RightBorderRoom;
    [SerializeField] static private SpawnPointsHolder spawnPointsHolder; 

    static private PrefabsStorage prefabsStorage;
    static int roomCounter;
    public Direction direction;
    public enum Direction{
        Top,
        Right,
        Left,
        Bottom,
        None
    }

    private void Awake() {
        if (spawnPointsHolder == null) {
            spawnPointsHolder = GameObject.FindGameObjectWithTag("Holder").GetComponent<SpawnPointsHolder>();
        }
        float randWaitTime = Random.Range(0.1f, 0.5f);
        if (direction != Direction.None) {
            spawnPointsHolder.spawnPoints.Add(gameObject);
        }
        if (prefabsStorage == null) {
            prefabsStorage = GameObject.FindGameObjectWithTag("Holder").GetComponent<PrefabsStorage>();
        }
    }

    public void Spawn() {
        int rand;
        if(!spawned) {
            GameObject newRoom = null;
            if (direction == Direction.Top ) {
                if (roomCounter < roomBorderNumber) {
                    rand = UnityEngine.Random.Range(0, prefabsStorage.topRooms.Count);
                    newRoom = Instantiate(prefabsStorage.topRooms[rand], transform.position, prefabsStorage.topRooms[rand].transform.rotation);
                } else {
                    newRoom = Instantiate(TopBorderRoom, transform.position, TopBorderRoom.transform.rotation);
                }
            } else if (direction == Direction.Right) {
                if (roomCounter < roomBorderNumber) {
                    rand = UnityEngine.Random.Range(0, prefabsStorage.rightRooms.Count);
                    newRoom = Instantiate(prefabsStorage.rightRooms[rand], transform.position, prefabsStorage.rightRooms[rand].transform.rotation);
                } else {
                    newRoom = Instantiate(RightBorderRoom, transform.position, RightBorderRoom.transform.rotation);
                }
            } else if (direction == Direction.Left) {
                if (roomCounter < roomBorderNumber) {
                    rand = UnityEngine.Random.Range(0, prefabsStorage.leftRooms.Count);
                    newRoom = Instantiate(prefabsStorage.leftRooms[rand], transform.position, prefabsStorage.leftRooms[rand].transform.rotation);
                } else {
                    newRoom = Instantiate(LeftBorderRoom, transform.position, LeftBorderRoom.transform.rotation);
                }
            } else if (direction == Direction.Bottom) {
                if (roomCounter < roomBorderNumber) {
                    rand = UnityEngine.Random.Range(0, prefabsStorage.bottomRooms.Count);
                    newRoom = Instantiate(prefabsStorage.bottomRooms[rand], transform.position, prefabsStorage.bottomRooms[rand].transform.rotation);
                } else {
                    newRoom = Instantiate(BottomBorderRoom, transform.position, BottomBorderRoom.transform.rotation);
                }
            }
            newRoom.transform.SetParent(gameObject.transform.parent);
            roomCounter++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Vector2 collisionPos = gameObject.transform.position;
        Direction thisPointDir = gameObject.GetComponent<RoomGenerator>().direction;
        if (other.CompareTag("RoomPoint")) {
            Direction otherPointDir = other.gameObject.GetComponent<RoomGenerator>().direction;
            if (otherPointDir != Direction.None && thisPointDir == Direction.None) {
                Destroy(other.gameObject);
            } 
            
            if (thisPointDir == Direction.None && other.gameObject.tag == "LowerPrioritySpawnPoint") {
                Debug.Log("Collision");

                Destroy(other.gameObject.transform.parent.gameObject);
                roomCounter--;
            }
        }
    }

}
