using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTrapController : MonoBehaviour {

    public float GasPeriod = 4.0f;
    public int StaminaDamage = 75;

    private bool isOn = false;
    private bool wasTriggered = false;
    private ParticleSystem gasMain;
    private ParticleSystem gasSmall;


    // Use this for initialization
    void Start() {
        // Gain references to particle system
        gasMain = transform.Find("mesh_SmogTrap/VFX_SmogTrap/VFX_SmogBig").GetComponent<ParticleSystem>();
        gasSmall = transform.Find("mesh_SmogTrap/VFX_SmogTrap/VFX_SmogSmall").GetComponent<ParticleSystem>();

        // Start cycling
        StartCoroutine(GasTimer());
    }


    void ToggleGas() {
        if (isOn) {
            // Turn off flame
            gasMain.Stop();
            // Hiss noise
        } else {
            // Turn on flame
            wasTriggered = true;
            gasMain.Play();
            // FIRE NOISE
        }
        isOn = !isOn;
    }

    private IEnumerator GasTimer() {
        yield return new WaitForSeconds(GasPeriod);
        ToggleGas();
        StartCoroutine(GasTimer());
    }

    public void OnTriggerEnter(Collider other) {
        if (!isOn) {
            return;
        }

        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().DrainStamina(StaminaDamage);
        }
    }

    public void OnTriggerStay(Collider other) {
        if (wasTriggered || !isOn) {
            return;
        }
        wasTriggered = true;

        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().DrainStamina(StaminaDamage);
        } 

    }

}
