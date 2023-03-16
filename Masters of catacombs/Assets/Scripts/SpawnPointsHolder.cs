using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsHolder : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    private Coroutine spawnRoomsCoroutine;
    private int index = 0;

    private void Start() {
        spawnRoomsCoroutine = StartCoroutine(spawnRooms());
    }

    public IEnumerator spawnRooms() {
        while(spawnPoints.Count != index) {
            if (spawnPoints[index] != null && spawnPoints[index].GetComponent<RoomGenerator>().spawned) {
                index++;
            }
            if (spawnPoints[index] != null) {
                spawnPoints[index].GetComponent<RoomGenerator>().Spawn();
            } else {
                index++;
            }
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("Finished");

        EventBus.onGenerationFinished?.Invoke();
        StopCoroutine(spawnRoomsCoroutine);
    }
}
