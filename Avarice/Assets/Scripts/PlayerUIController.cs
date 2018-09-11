using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    static PlayerUIController instance;

    [SerializeField]
    private Text coinText;
    [SerializeField]
    private Slider healthBar;


	// Use this for initialization
	void Start () {
        instance = this;
        healthBar.maxValue = PlayerController.instance.iLife;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void UpdateCoinText(int _iCurrentCoins) {
        instance.coinText.text = "COINS: " + _iCurrentCoins.ToString();
    }

    public static void UpdateHealthSlider(int _iCurrentHealth) {
        instance.healthBar.value = _iCurrentHealth;
    }

    
}
