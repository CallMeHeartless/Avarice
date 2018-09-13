using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScripts : MonoBehaviour {

    PlayerController player;

    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

	public void ThrowCoin() {
        player.CreateCoinPileDistraction();
    }

    public void TurnUndead() {
        player.TurnUndead();
    }
}
