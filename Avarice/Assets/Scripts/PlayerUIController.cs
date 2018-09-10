using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    static PlayerUIController instance;

    [SerializeField]
    private Text coinText;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void UpdateCoinText(int _iCurrentCoins) {
        instance.coinText.text = "COINS: " + _iCurrentCoins.ToString();
    }

    
}
