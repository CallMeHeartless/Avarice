using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private static bool isAfraid = false;

    // Use this for initialization
    void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {

    }

    public static void TurnUndead(float _fDuration) {
        isAfraid = true;
        instance.StartCoroutine(EndTurnUndead(_fDuration));
    }

    static IEnumerator EndTurnUndead(float _fDuration) {
        yield return new WaitForSeconds(_fDuration);
        isAfraid = false;
    }

    public static bool AreUndeadTurned(){
        return isAfraid;
    }
}
