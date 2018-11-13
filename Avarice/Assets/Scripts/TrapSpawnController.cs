using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawnController : MonoBehaviour {

    public GameObject prefab;

    private Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
        spawnPoints = GetComponentsInChildren<Transform>();
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject trap = GameObject.Instantiate(prefab, spawnPoints[spawnIndex]) as GameObject;
        trap.transform.position = trap.transform.GetChild(0).transform.position;
	}
}
