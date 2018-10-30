using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressionController : MonoBehaviour {

    private static PlayerProgressionController instance = null;
    [SerializeField]
    private bool testMode = false;
    public int testCoins = 5000;

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

    // Stamina
    [Header("Stamina")]
    [SerializeField]
    private float[] StaminaValues = { 100.0f, 125.0f, 150.0f, 175.0f, 200.0f, 250.0f };
    [SerializeField]
    private int[] StaminaUpgradeCosts = { 1000, 2000, 6000, 8000, 10000 };
    [SerializeField]
    private int StaminaIndex = 0;

    // Turn Undead Uses
    [Header("Turn Undead Uses")]
    [SerializeField]
    private int[] TurnUndeadUseValues = { 1, 2, 3, 4, 5 };
    [SerializeField]
    private int[] TurnUndeadUpgradeCosts = { 2500, 5000, 10000, 12000 };
    [SerializeField]
    private int TurnUndeadIndex = 0;

    // Damage Multiplier Values
    [Header("Damage Multiplier Values")]
    [SerializeField]
    private float[] DamageMultiplierValues = { 1.0f, 1.25f, 1.5f, 1.75f, 2.0f, 2.5f };
    [SerializeField]
    private int[] DamageUpgradeCosts = { 2000, 4000, 8000, 12000, 18000 };
    [SerializeField]
    private int DamageIndex = 0;

    // Coin Multiplier values
    [Header("Coin Multiplier Values")]
    [SerializeField]
    private float[] CoinMultiplierValues = { 1.0f, 1.05f, 1.10f, 1.15f, 1.20f, 1.25f };
    [SerializeField]
    private int[] CoinUpgradeCosts = { 1500, 2500, 8000, 10000, 20000 };
    [SerializeField]
    private int CoinMultiplierIndex = 0;

	/// Use this for initialization
	void Awake () {
        // Prevent more than one instance from existing
        if (instance != null) {
            Destroy(instance.gameObject);
        }
        instance = this;
        // If not running hard values (testing), then load the player's indices
        if (!testMode) {
            InitialiseIndices();
        } else {

        }
	}
	
	/// Update is called once per frame
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

    /// Loads the player's index of each property
    private void InitialiseIndices() {
        ToxicityIndex = PlayerPrefs.GetInt("ToxicityIndex", 0);
        HealthIndex = PlayerPrefs.GetInt("HealthIndex", 0);
        StaminaIndex = PlayerPrefs.GetInt("StaminaIndex", 0);
        TurnUndeadIndex = PlayerPrefs.GetInt("TurnUndeadIndex", 0);
        DamageIndex = PlayerPrefs.GetInt("DamageIndex", 0);
        CoinMultiplierIndex = PlayerPrefs.GetInt("CoinMultiplierIndex", 0);
    }

    /// Increase the player's toxicity resistance index if possible (scene will need to be updated following this call)
    public static bool UpgradeToxicity() {
        // Check player is not at maximum 
        if(instance.ToxicityIndex >= instance.ToxicityValues.Length) {
            Debug.Log("ERROR: Cannot increment Toxicity - maximum level reached");
            return false;
        }
        int UpgradeCost = instance.ToxicityUpgradeCosts[instance.ToxicityIndex];
        // Check player has enough coin
        if (CheckPlayerCanPurchase(UpgradeCost)) {
            ++instance.ToxicityIndex;
            PlayerPrefs.SetInt("ToxicityIndex", instance.ToxicityIndex);
            DeductCoins(UpgradeCost);
            return true;
        } else {
            Debug.Log("ERROR: Could not buy toxicity resistance - insufficient coins");
            return false;
        }

    }

    /// Get toxicity index
    public static int GetToxicityIndex() {
        return instance.ToxicityIndex;
    }

    public static float GetToxicity() {
        return instance.ToxicityValues[instance.ToxicityIndex];
    }

    /// Upgrade the player's health level if possible
    public static bool UpgradeHealth() {
        // Check player is not at maximum 
        if (instance.HealthIndex >= instance.HealthValues.Length) {
            Debug.Log("ERROR: Cannot increment Health - maximum level reached");
            return false;
        }
        int UpgradeCost = instance.HealthUpgradeCosts[instance.HealthIndex];
        // Check player has enough coin
        if (CheckPlayerCanPurchase(UpgradeCost)) {
            ++instance.HealthIndex;
            PlayerPrefs.SetInt("HealthIndex", instance.HealthIndex);
            DeductCoins(UpgradeCost);
            return true;
        } else {
            Debug.Log("ERROR: Could not buy health - insufficient coins");
            return false;
        }
    }

    public static int GetHealthIndex() {
        return instance.HealthIndex;
    }

    public static int GetHealth() {
        return instance.HealthValues[instance.HealthIndex];
    }

    /// Upgrade player's stamina
    public static bool UpgradeStamina() {
        // Check player is not at maximum 
        if (instance.StaminaIndex >= instance.StaminaValues.Length) {
            Debug.Log("ERROR: Cannot increment Stamina - maximum level reached");
            return false;
        }
        int UpgradeCost = instance.StaminaUpgradeCosts[instance.StaminaIndex];
        // Check player has enough coin
        if (CheckPlayerCanPurchase(UpgradeCost)) {
            ++instance.StaminaIndex;
            PlayerPrefs.SetInt("StaminaIndex", instance.StaminaIndex);
            DeductCoins(UpgradeCost);
            return true;
        } else {
            Debug.Log("ERROR: Could not buy Stamina - insufficient coins");
            return false;
        }
    }

    public static int GetStaminaIndex() {
        return instance.StaminaIndex;
    }

    public static float GetStamina() {
        return instance.StaminaValues[instance.StaminaIndex];
    }

    /// Upgrade damage
    public static bool UpgradeDamage() {
        // Check player is not at maximum 
        if (instance.DamageIndex >= instance.DamageMultiplierValues.Length) {
            Debug.Log("ERROR: Cannot increment Damage - maximum level reached");
            return false;
        }
        int UpgradeCost = instance.DamageUpgradeCosts[instance.DamageIndex];
        // Check player has enough coin
        if (CheckPlayerCanPurchase(UpgradeCost)) {
            ++instance.DamageIndex;
            PlayerPrefs.SetInt("DamageIndex", instance.DamageIndex);
            DeductCoins(UpgradeCost);
            return true;
        } else {
            Debug.Log("ERROR: Could not buy Damage - insufficient coins");
            return false;
        }
    }

    public static int GetDamageIndex() {
        return instance.DamageIndex;
    }

    public static float GetDamage() {
        return instance.DamageMultiplierValues[instance.DamageIndex];
    }

    /// Turn Undead Upgrade
    public static bool UpgradeTurnUndead() {
        // Check player is not at maximum 
        if (instance.TurnUndeadIndex >= instance.TurnUndeadUseValues.Length) {
            Debug.Log("ERROR: Cannot increment Turn Undead - maximum level reached");
            return false;
        }
        int UpgradeCost = instance.TurnUndeadUpgradeCosts[instance.TurnUndeadIndex];
        // Check player has enough coin
        if (CheckPlayerCanPurchase(UpgradeCost)) {
            ++instance.TurnUndeadIndex;
            PlayerPrefs.SetInt("TurnUndeadIndex", instance.TurnUndeadIndex);
            DeductCoins(UpgradeCost);
            return true;
        } else {
            Debug.Log("ERROR: Could not buy Turn Undead - insufficient coins");
            return false;
        }
    }

    public static int GetTurnUndeadIndex() {
        return instance.TurnUndeadIndex;
    }

    public static int GetTurnUndead() {
        return instance.TurnUndeadUseValues[instance.TurnUndeadIndex];
    }

    /// Coin Multiplier Upgrade
    public static bool UpgradeCoinMultiplier() {
        // Check player is not at maximum 
        if (instance.CoinMultiplierIndex >= instance.CoinMultiplierValues.Length) {
            Debug.Log("ERROR: Cannot increment Turn Undead - maximum level reached");
            return false;
        }
        int UpgradeCost = instance.CoinUpgradeCosts[instance.CoinMultiplierIndex];
        // Check player has enough coin
        if (CheckPlayerCanPurchase(UpgradeCost)) {
            ++instance.CoinMultiplierIndex;
            PlayerPrefs.SetInt("CoinMultiplierIndex", instance.CoinMultiplierIndex);
            DeductCoins(UpgradeCost);
            return true;
        } else {
            Debug.Log("ERROR: Could not buy Turn Undead - insufficient coins");
            return false;
        }
    }

    public static int GetCoinMultiplierIndex() {
        return instance.CoinMultiplierIndex;
    }

    public static float GetCoinMultiplier() {
        return instance.CoinMultiplierValues[instance.CoinMultiplierIndex];
    }

    public static int[] GetUpgradeCosts(string property) {
        if(property == "Toxicity") {
            return instance.ToxicityUpgradeCosts;
        }else if(property == "Health") {
            return instance.HealthUpgradeCosts;
        }else if(property == "Stamina") {
            return instance.StaminaUpgradeCosts;
        }else if(property == "TurnUndead") {
            return instance.TurnUndeadUpgradeCosts;
        }else if(property == "Damage") {
            return instance.DamageUpgradeCosts;
        }else if(property == "Coin") {
            return instance.CoinUpgradeCosts;
        } else {
            Debug.LogError("ERROR: Progression Property not recognised. Null array returned.");
            return null;
        }
    }

    // Reset all player progression
    public static void ResetAll() {
        PlayerPrefs.SetInt("TotalCoins", 0);
        PlayerPrefs.SetInt("ToxicityIndex", 0);
        PlayerPrefs.SetInt("HealthIndex", 0);
        PlayerPrefs.SetInt("StaminaIndex", 0);
        PlayerPrefs.SetInt("TurnUndeadIndex", 0);
        PlayerPrefs.SetInt("DamageIndex", 0);
        PlayerPrefs.SetInt("CoinMultiplierIndex", 0);
    }
}
