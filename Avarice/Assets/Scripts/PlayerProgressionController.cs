using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressionController : MonoBehaviour {

    private static PlayerProgressionController instance = null;
    [SerializeField]
    private bool testMode = false;

    // Toxicity
    [Header("Toxicity")]
    [SerializeField]
    private float[] ToxicityValues = { 180.0f, 200.0f, 220.0f, 250.0f, 275.0f, 300.0f};
    [SerializeField]
    private int[] ToxicityUpgradeCosts = { 1000, 2000, 6000, 8000, 10000 };
    [SerializeField]
    private int ToxicityIndex = 0;

    // Health
    [Header("Health")]
    [SerializeField]
    private int[] HealthValues = { 100, 125, 150, 175, 200, 250 };
    [SerializeField]
    private int[] HealthUpgradeCosts = { 1000, 2000, 6000, 8000, 10000 };
    [SerializeField]
    private int HealthIndex = 0;

    // 
    [Header("Stamina")]
    [SerializeField]
    private float[] StaminaValues = { 100.0f, 125.0f, 150.0f, 175.0f, 200.0f, 250.0f };
    [SerializeField]
    private int[] StaminaCostValues = { 1000, 2000, 6000, 8000, 10000 };
    [SerializeField]
    private int StaminaIndex = 0;


    private int[] TurnUndeadUseValues = { 1, 2, 3, 4, 5 };
    private float[] DamageMultiplierValues = { 1.0f, 1.25f, 1.5f, 1.75f, 2.0f, 2.5f };
    private float[] CoinMultiplierValues = { 1.0f, 1.05f, 1.10f, 1.15f, 1.20f, 1.25f };

	// Use this for initialization
	void Start () {
        // Prevent more than one instance from existing
        if(instance != null) {
            Destroy(instance.gameObject);
        }
        instance = this;
        if (!testMode) {
            
        }
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

    private void InitialiseIndices() {
        ToxicityIndex = PlayerPrefs.GetInt("ToxicityIndex");

    }
}
