using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScripts : MonoBehaviour {

    PlayerController player;

    void Awake() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

	public void ThrowCoin() {
        player.CreateCoinPileDistraction();
    }

    public void TurnUndead() {
        player.TurnUndead();
    }

    public void EndGame() {
        StartCoroutine(DeathCountdown());
    }

    private IEnumerator DeathCountdown() {
        yield return new WaitForSeconds(2.0f);
        PlayerController.ReturnToMainMenu();
    }
}
