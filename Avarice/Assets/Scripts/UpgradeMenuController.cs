using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuController : MonoBehaviour {

    private Text coinText;

    // Buttons for selecting different upgrades
    private Transform toxicityButtons;
    private Transform healthButtons;
    private Transform staminaButtons;
    private Transform turnUndeadButtons;
    private Transform damageButtons;
    private Transform coinButtons;

    // Details for each upgrade
    private Transform toxicityLevels;
    private Transform healthLevels;
    private Transform staminaLevels;
    private Transform turnUndeadLevels;
    private Transform damageLevels;
    private Transform coinLevels;

    // Use this for initialization
    void Start () {
        // Update coins
        coinText = transform.Find("BackgroundImage/CoinIcon/Text").GetComponent<Text>();
        coinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();

        // Get parent transforms
        toxicityButtons = transform.Find("ToxicityTime");
        healthButtons = transform.Find("Health");
        staminaButtons = transform.Find("Stamina");
        turnUndeadButtons = transform.Find("TurnUndead");
        damageButtons = transform.Find("Damage");
        coinButtons = transform.Find("CoinMultiplier");

        toxicityLevels = transform.Find("ToxicityTimeLevels");
        healthLevels = transform.Find("HealthLevels");
        staminaLevels = transform.Find("StaminaLevels");
        turnUndeadLevels = transform.Find("TurnUndeadLevels");
        damageLevels = transform.Find("DamageLevels");
        coinLevels = transform.Find("CoinMultiplierLevels");

        // Initialise values
        SetUpToxicity();
        SetUpDamage();
        SetUpHealth();
        SetUpStamina();
        SetUpTurnUndead();
        SetUpCoinMultiplier();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AttemptToPurchase(string item) {
        if(item == "Toxicity") {
            if (PlayerProgressionController.UpgradeToxicity()) {
                coinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
                // Play audio

                // Rebuild menu
                SetUpToxicity();
            }
        } 
        else if (item == "Health") {
            if (PlayerProgressionController.UpgradeHealth()) {
                coinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
                // Play audio

                // Rebuild menu
                SetUpHealth();
            }
        } 
        else if (item == "Stamina") {
            if (PlayerProgressionController.UpgradeStamina()) {
                coinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
                // Play audio

                // Rebuild menu
                SetUpStamina();
            }
        } 
        else if (item == "TurnUndead") {
            if (PlayerProgressionController.UpgradeTurnUndead()) {
                coinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
                // Play audio

                // Rebuild menu
                SetUpTurnUndead();
            }
        } 
        else if (item == "Damage") {
            if (PlayerProgressionController.UpgradeDamage()) {
                coinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
                // Play audio

                // Rebuild menu
                SetUpDamage();
            }
        }
        else if (item == "Coin") {
            if (PlayerProgressionController.UpgradeCoinMultiplier()) {
                coinText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
                // Play audio

                // Rebuild menu
                SetUpCoinMultiplier();
            }
        } else {
            // Play error noise
            Debug.Log("Could not purchase");
        }

    }

    private void SetUpToxicity() {
        int playerToxicityIndex = PlayerProgressionController.GetToxicityIndex();
        int[] playerToxicityCosts = PlayerProgressionController.GetUpgradeCosts("Toxicity");
        // Iterate through each child and set cost
        for(int i = 0; i < playerToxicityCosts.Length; ++i) {
            Transform childI = toxicityLevels.GetChild(i);
            // If the player hasn't purchased the upgrade, set the cost
            if(i > playerToxicityIndex) {
                childI.Find("Cost").GetComponent<Text>().text = playerToxicityCosts[i].ToString();
            } else {
                // Else mark as purchased
                childI.Find("Cost").GetComponent<Text>().text = "PURCHASED";
            }

            if (i != playerToxicityIndex + 1){
               
                Destroy(childI.Find("BuyButton").gameObject);
            }
        }
    }

    private void SetUpHealth() {
        int playerHealthIndex = PlayerProgressionController.GetHealthIndex();
        int[] playerHealthCosts = PlayerProgressionController.GetUpgradeCosts("Health");
        // Iterate through each child and set cost
        for (int i = 0; i < playerHealthCosts.Length; ++i) {
            Transform childI = healthLevels.GetChild(i);
            // If the player hasn't purchased the upgrade, set the cost
            if (i > playerHealthIndex) {
                childI.Find("Cost").GetComponent<Text>().text = playerHealthCosts[i].ToString();
            } else {
                // Else mark as purchased
                childI.Find("Cost").GetComponent<Text>().text = "PURCHASED";
                Destroy(childI.Find("BuyButton").gameObject);
            }
        }
    }

    private void SetUpStamina() {
        int itemIndex = PlayerProgressionController.GetStaminaIndex();
        int[] Costs = PlayerProgressionController.GetUpgradeCosts("Stamina");
        // Iterate through each child and set cost
        for (int i = 0; i < Costs.Length; ++i) {
            Transform childI = staminaLevels.GetChild(i);
            // If the player hasn't purchased the upgrade, set the cost
            if (i > itemIndex) {
                childI.Find("Cost").GetComponent<Text>().text = Costs[i].ToString();
            } else {
                // Else mark as purchased
                childI.Find("Cost").GetComponent<Text>().text = "PURCHASED";
                Destroy(childI.Find("BuyButton").gameObject);
            }
        }
    }

    private void SetUpTurnUndead() {
        int itemIndex = PlayerProgressionController.GetTurnUndeadIndex();
        int[] Costs = PlayerProgressionController.GetUpgradeCosts("TurnUndead");
        // Iterate through each child and set cost
        for (int i = 0; i < Costs.Length; ++i) {
            Transform childI = turnUndeadLevels.GetChild(i);
            // If the player hasn't purchased the upgrade, set the cost
            if (i > itemIndex) {
                childI.Find("Cost").GetComponent<Text>().text = Costs[i].ToString();
            } else {
                // Else mark as purchased
                childI.Find("Cost").GetComponent<Text>().text = "PURCHASED";
                Destroy(childI.Find("BuyButton").gameObject);
            }
        }
    }

    private void SetUpDamage() {
        int itemIndex = PlayerProgressionController.GetDamageIndex();
        int[] Costs = PlayerProgressionController.GetUpgradeCosts("Damage");
        // Iterate through each child and set cost
        for (int i = 0; i < Costs.Length; ++i) {
            Transform childI = damageLevels.GetChild(i);
            // If the player hasn't purchased the upgrade, set the cost
            if (i > itemIndex) {
                childI.Find("Cost").GetComponent<Text>().text = Costs[i].ToString();
            } else {
                // Else mark as purchased
                childI.Find("Cost").GetComponent<Text>().text = "PURCHASED";
                Destroy(childI.Find("BuyButton").gameObject);
            }
        }
    }

    private void SetUpCoinMultiplier() {
        int itemIndex = PlayerProgressionController.GetCoinMultiplierIndex();
        int[] Costs = PlayerProgressionController.GetUpgradeCosts("Coin");
        // Iterate through each child and set cost
        for (int i = 0; i < Costs.Length; ++i) {
            Transform childI = coinLevels.GetChild(i);
            // If the player hasn't purchased the upgrade, set the cost
            if (i > itemIndex) {
                childI.Find("Cost").GetComponent<Text>().text = Costs[i].ToString();
            } else {
                // Else mark as purchased
                childI.Find("Cost").GetComponent<Text>().text = "PURCHASED";
                Destroy(childI.Find("BuyButton").gameObject);
            }
        }
    }


}
