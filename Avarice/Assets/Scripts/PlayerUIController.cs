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
    [SerializeField]
    private Text currentScoreText;
    [SerializeField]
    private Text highScoreText;



	// Use this for initialization
	void Start () {
        instance = this;
        highScoreText.text = "High Tribute: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
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

    public static void SetHealthSliderMaxValue(int _iMaxLife) {
        instance.healthBar.maxValue = _iMaxLife;
        instance.healthBar.value = _iMaxLife;
    }

    public static void UpdateCurrentTributeText(int _iTributeValue) {
        instance.currentScoreText.text = "Tribute: " + _iTributeValue.ToString();
    }

    public static void UpdateHighScoreText() {
        instance.highScoreText.text = "High Tribute: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    
}
