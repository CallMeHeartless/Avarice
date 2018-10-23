using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    private static PlayerController instance;

    [Header("Player Progression Variables")]
    private int iLife = 100;
    private float fPlayerStamina = 100;
    private float fPlayerToxicity = 180;
    private float fPlayerToxicityCounter = 0.0f;
    private int iTurnUndeadUses = 1;
    private int iCoinCount = 0;
    private float fCoinMultiplier = 1.0f;
    private int iDamage = 1;

    [Header("Player Control Variables")]
    public float fSpeed = 5.0f;
    public float flookSensitivity = 10.0f;
    public float fAttackRate = 0.6f;
    public float fBleedInterval = 10.0f;
    public int iBleedAmount = 10;
    public static bool bIsAttacking = false;
    public float fTurnDuration = 10.0f;

    [SerializeField]
    private int iCoinDistractionCost = 50;
    //[SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform coinThrow;
    [SerializeField]
    private ParticleSystem turnUndeadParticles;
    [SerializeField]
    private AudioSource coinStep;
    

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotation = 0.0f;
    private float currentCameraRotation = 0.0f;
    private Camera _camera;
    private float fBleedCounter = 0.0f;

	// Use this for initialization
	void Start () {
        instance = this;
        rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        anim = GetComponentInChildren<Animator>();
        // PlayerUIController.SetHealthSliderMaxValue(iLife);
        LoadPlayerVariables();
	}
	
	// Update is called once per frame
	void Update () {
		if(iLife <= 0) {
            return;
        }

        SetMovement();
        BleedPlayer();

        if(Input.GetButton("Fire2")) { // Default key for now
            //CreateCoinPileDistraction();
            if(iCoinCount > 0) {
                anim.SetTrigger("Throw");
            }
            
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(iTurnUndeadUses > 0) {
                anim.SetTrigger("LiftSword");
            }
            //TurnUndead();
        }

        // Attack
        if(!bIsAttacking && Input.GetButton("Fire1")) {
            Attack();
        }

        //ResetAnimations();
    }

    void FixedUpdate() {
        if(velocity != Vector3.zero) {
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }
        if(rotation != Vector3.zero) {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        }
        if (_camera != null && cameraRotation != 0.0f) {
            currentCameraRotation -= cameraRotation;
            currentCameraRotation = Mathf.Clamp(currentCameraRotation, -85, 85);
            _camera.transform.localEulerAngles = new Vector3(currentCameraRotation, 0, 0);
        }

    }

    // Determines variables used for player movement and camera rotation
    private void SetMovement() {
        // Set movement
        Vector3 forwardMovement = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 sideMovement = transform.right * Input.GetAxisRaw("Horizontal");

        velocity = (forwardMovement + sideMovement).normalized * fSpeed;
        if(velocity.sqrMagnitude > 0) {
            anim.SetTrigger("Run");
            if (!coinStep.isPlaying) {
                coinStep.Play();
            }
        } else {
            anim.SetTrigger("Idle");
            if (coinStep.isPlaying) {
                coinStep.Stop();
            }
        }

        // Set rotation
        rotation = new Vector3(0.0f, Input.GetAxisRaw("Mouse X"), 0.0f) * flookSensitivity;

        // Camera rotation 
        cameraRotation = Input.GetAxisRaw("Mouse Y") * flookSensitivity;
    }

    // Damages the player's health in set intervals.
    private void BleedPlayer() {
        fBleedCounter += Time.deltaTime;
        if(fBleedCounter >= fBleedInterval) {
            fBleedCounter = 0.0f;
            DamagePlayer(iBleedAmount);
        }
    }

    public void DamagePlayer(int _iDamage) {
        iLife -= _iDamage;
        PlayerUIController.UpdateHealthSlider(iLife);
        if(iLife <= 0) {
            SceneManager.LoadScene(0);
        }
    }

    // Throws coins out as a distraction
    public void CreateCoinPileDistraction() {
        if(iCoinCount >= iCoinDistractionCost) {
            iCoinCount -= iCoinDistractionCost;
            GameObject distraction = Instantiate( Resources.Load("Coin Pile Distraction", typeof(GameObject))) as GameObject;
            distraction.transform.position = transform.position + Vector3.up* 0.2f + transform.forward * 0.1f;
            distraction.GetComponent<Rigidbody>().AddForce(_camera.transform.forward * 10, ForceMode.Impulse);
            PlayerUIController.UpdateCoinText(iCoinCount);
        }
    }

    // Turns undead, making enemies flee the player for a short time
    public void TurnUndead() {
        if(iTurnUndeadUses > 0) {
            --iTurnUndeadUses;
            turnUndeadParticles.Play(true);
            // Call static game manager function
            GameManager.TurnUndead(fTurnDuration);
        }
    }

    // Adds coins to the players inventory, then directs game manager to update based on new total
    public void AddCoinsToInventory(int _iCoinValue) {
        iCoinCount += _iCoinValue;
        // Update game manager with player's new current coin count
        GameManager.PlayerCollectedCoins(iCoinCount);

        // Update text
        PlayerUIController.UpdateCoinText(iCoinCount);
    }

    public static int PayTribute() {
        int temp = instance.iCoinCount;
        instance.iCoinCount = 0;
        PlayerUIController.UpdateCoinText(0);
        return temp;
    }

    private void Attack() {
        bIsAttacking = true;
        // Animation
        anim.SetTrigger("Attack");
        // cooldown
        StartCoroutine(AttackCooldown(fAttackRate));
    }

    IEnumerator AttackCooldown(float _fAttackCooldown) {
        yield return new WaitForSeconds(_fAttackCooldown);
        bIsAttacking = false;
    }

    private void ResetAnimations() {
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("Attack");
        anim.ResetTrigger("LiftSword");
        anim.ResetTrigger("Pickup");
        anim.ResetTrigger("Run");
        anim.ResetTrigger("Throw");
    }

    private void LoadPlayerVariables() {

    }

}
