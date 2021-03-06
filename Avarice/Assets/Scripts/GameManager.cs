﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private static bool isAfraid = false;
    private static int iTributeTotal = 0;
    private static int iCoinsOnPlayer = 0;

    // Use this for initialization
    void Start() {
        instance = this;
        SpawnCoins();
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {

    }

    //Spawn enemies after tribute collection
    public static void SpawnEnemies()
    {
        GameObject[] gos;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        gos = GameObject.FindGameObjectsWithTag("eSpawn");
        GameObject closest = null;
        float distance = 100000.0f;
        Vector3 position = player.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        string groupName = closest.GetComponent<eSpawn>().groupName;

        foreach(GameObject go in gos)
        {
            if(groupName == go.GetComponent<eSpawn>().groupName)
            {
                GameObject SkullBoi = Instantiate(Resources.Load("SkellyMan", typeof(GameObject))) as GameObject;
                SkullBoi.GetComponent<EnemyAI>().agent.Warp(go.transform.position);
                //SkullBoi.transform.position = go.transform.position;
            }
        }
    }

    // Makes undead afraid of the player
    public static void TurnUndead(float _fDuration) {
        isAfraid = true;
        instance.StartCoroutine(EndTurnUndead(_fDuration));
        // Stun all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies) {
            enemy.GetComponent<EnemyAI>().StunEnemy(_fDuration);
        }
    }

    // Coroutine to end effects of undead being afraid of the player after the set duration
    static IEnumerator EndTurnUndead(float _fDuration) {
        yield return new WaitForSeconds(_fDuration);
        isAfraid = false;
    }

    // Returns true if undead should flee the player
    // Enemies should call this function before processing their target each frame
    public static bool AreUndeadTurned(){
        return isAfraid;
    }

    // Call to add coins to the player's current tribute total for this life
    public static void AddCoinsToTribute(int _iCoinAmount) {
        iTributeTotal += _iCoinAmount;
        PlayerUIController.UpdateCurrentTributeText(iTributeTotal);
        if(iTributeTotal > PlayerPrefs.GetInt("HighScore", 0)) {
            UpdateHighScore(iTributeTotal);
        }
        // Reset coins on player
        iCoinsOnPlayer = 0;
        // Reset Level
        DestroyAllSkeletons();
        DestroyAllCoins();
        SpawnCoins();
    }

    private static void DestroyAllSkeletons() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies) {
            Destroy(enemy);
        }
    }

    // Updates the number of coins the player has on them and spawns enemies as needed.
    public static void PlayerCollectedCoins(int _iPlayerCoinTotal) {
        if(_iPlayerCoinTotal > iCoinsOnPlayer) {
            // Only spawn more enemies if the player has collected more coins
            SpawnEnemies();
        }


        // Update total coins on player
        iCoinsOnPlayer = _iPlayerCoinTotal;
    }

    public static void UpdateHighScore(int _iNewHighScore) {
        // Update storage
        PlayerPrefs.SetInt("HighScore", _iNewHighScore);
        // Update text
        PlayerUIController.UpdateHighScoreText();
    }

    private static void DestroyAllCoins() {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("CoinPickup");
        foreach(GameObject coin in coins) {
            Destroy(coin);
        }
    }

    private static void SpawnCoins() {
        GameObject[] coinSpawns = GameObject.FindGameObjectsWithTag("RoomCoinSpawn");
        foreach(GameObject coinSpawn in coinSpawns) {
            coinSpawn.GetComponent<RoomCoinSpawnController>().SpawnCoin();
        }
    }
}
