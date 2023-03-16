using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMesh;

    private void OnEnable() {
        EventBus.onGenerationFinished += RebakeNavMesh;
    }

    private void OnDisable() {
        EventBus.onGenerationFinished -= RebakeNavMesh;
    }


    private void RebakeNavMesh() {
        navMesh.BuildNavMesh();
        Debug.Log("Rebake Finished");
    }
}
