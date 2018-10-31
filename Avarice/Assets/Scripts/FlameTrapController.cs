using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrapController : MonoBehaviour {

    public float FirePeriod = 4.0f;
    public int FireDamage = 25;

    private bool isOn = false;
    private ParticleSystem flameMain;
    private ParticleSystem flameMainAux;
    private ParticleSystem flameSparks;
    private ParticleSystem flameGlow;

	// Use this for initialization
	void Start () {
        // Gain references to particle system
        flameMain = transform.Find("mesh_FlameThrower/VFX_FlameThrowerParent/VFX_Fire_ALPHA").GetComponent<ParticleSystem>();
        flameMainAux = transform.Find("mesh_FlameThrower/VFX_FlameThrowerParent/VFX_Fire_ADD").GetComponent<ParticleSystem>();
        flameSparks = transform.Find("mesh_FlameThrower/VFX_FlameThrowerParent/VFX_Sparks").GetComponent<ParticleSystem>();
        flameGlow = transform.Find("mesh_FlameThrower/VFX_FlameThrowerParent/VFX_Glow").GetComponent<ParticleSystem>();

        // Start cycling
        StartCoroutine(FlamePeriod());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ToggleFlame() {
        if (isOn) {
            // Turn off flame
            flameMain.Stop();
            flameMainAux.Stop();
            // Hiss noise
        } else {
            // Turn on flame
            flameMain.Play();
            flameMainAux.Play();
            // FIRE NOISE
        }
        isOn = !isOn;
    }

    private IEnumerator FlamePeriod() {
        yield return new WaitForSeconds(FirePeriod);
        ToggleFlame();
        StartCoroutine(FlamePeriod());
    }

    public void OnTriggerEnter(Collider other) {
        if (!isOn) {
            return;
        }

        if (other.CompareTag("Player")) {
            Debug.Log("Hit player");
            other.GetComponent<PlayerController>().DamagePlayer(FireDamage);
        }
    }

}
