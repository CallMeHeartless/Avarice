using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressionController : MonoBehaviour {

    private static PlayerProgressionController instance = null;

	// Use this for initialization
	void Start () {
        // Prevent more than one instance from existing
        if(instance != null) {
            Destroy(instance.gameObject);
        }
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void AddToTotalCoins(int _iCoins) {
        int iNewTotal = PlayerPrefs.GetInt("TotalCoins") + _iCoins;
        PlayerPrefs.SetInt("TotalCoins", iNewTotal);
    }

    public static void DeductCoins(int _iCoins) {
        int iNewTotal = PlayerPrefs.GetInt("TotalCoins") - _iCoins;
        PlayerPrefs.SetInt("TotalCoins", iNewTotal);
    }

    public static bool CheckPlayerCanPurchase(int _iPurchaseCost) {
        return _iPurchaseCost <= PlayerPrefs.GetInt("TotalCoins");
    }
}
