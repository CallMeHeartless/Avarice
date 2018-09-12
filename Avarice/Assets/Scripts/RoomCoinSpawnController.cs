using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCoinSpawnController : MonoBehaviour {

    [SerializeField]
    Transform[] coinSpawnPoints;

    public bool bIsEdgeCase = false;

    public void SpawnCoin() {
        if (!bIsEdgeCase) {
            int spawnPointIndex = Random.Range(0, coinSpawnPoints.Length);

            GameObject coin = Instantiate(Resources.Load("Coin Pickup", typeof(GameObject))) as GameObject;
            coin.transform.position = coinSpawnPoints[spawnPointIndex].position;
        } else {
            foreach(Transform point in coinSpawnPoints) {
                GameObject coin = Instantiate(Resources.Load("Coin Pickup", typeof(GameObject))) as GameObject;
                coin.transform.position = point.position;
            }
        }

    }

}
