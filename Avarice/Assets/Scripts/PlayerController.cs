using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   

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
    private float fDamageMod = 1.0f;

    [Header("Player Control Variables")]
    public float fSpeed = 5.0f;
    public float flookSensitivity = 10.0f;
    public float fAttackRate = 0.6f;
    private int iAttackCount = 0;
    public float fBleedInterval = 10.0f;
    public int iBleedAmount = 10;
    public float fStaminaDrainLight = 15.0f;
    public float fStaminaDrainHeavy = 25.0f;
    public float fSprintMultiplier = 1.6f;
    public float fStaminaRegainDelay = 2.0f;
    private bool bIsRegainingStamina = false;
    public float fStaminaRegain = 3.0f;
    public float fSprintDrain = 10.0f;
    public static bool bIsAttacking = false;
    public float fTurnDuration = 10.0f;
    private float fPlayerStaminaCounter = 100.0f;
    public GameObject PlayerSword;
    public Collider SwordCollider;
    private bool isDead = false;

    [SerializeField]
    private int iCoinDistractionCost = 100;
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

    // UI Variables
    private Image toxicityMeter;
    private Slider healthMeter;
    private Slider staminaMeter;
    private Text turnUndeadText;
    private Text coinText;
    private Slider compass;
    private Transform compassTarget;

    // Use this for initialization
    void Start () {
        instance = this;
        rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        anim = GetComponentInChildren<Animator>();
        LoadPlayerVariables();
        InitialisePlayerUI();
        SwordCollider = PlayerSword.GetComponent<BoxCollider>();
        SwordCollider.enabled = false;
        // Lock camera to screen
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        // Get test target
        compassTarget = GameObject.Find("TributePile").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if(iLife <= 0) {
            return;
        }

        SetMovement();


        if(Input.GetButtonDown("Fire2")) { // Default key for now
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
        if(Input.GetButtonDown("Fire1") && fPlayerStamina > fStaminaDrainLight) {
             Attack();
        }

        // Constant stamina regen
        if(fPlayerStaminaCounter < fPlayerStamina && bIsRegainingStamina) {
            fPlayerStaminaCounter += Time.deltaTime * fStaminaRegain;
            if(fPlayerStaminaCounter > fPlayerStamina) {
                fPlayerStaminaCounter = fPlayerStamina;
            }
        }
        // Update slider
        staminaMeter.value = fPlayerStaminaCounter;

        // Constant toxicity raise
        if (fPlayerToxicityCounter < fPlayerToxicity) {
            fPlayerToxicityCounter += Time.deltaTime;
            if(fPlayerToxicityCounter > fPlayerToxicity) {
                fPlayerToxicityCounter = fPlayerToxicity;
            }
            // Update slider
            toxicityMeter.fillAmount = fPlayerToxicityCounter / fPlayerToxicity;
        } else {
            BleedPlayer();
        }

        // Update compass
        if(compass && compassTarget) {
            UpdateCompass();
        }
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
        if(Input.GetKey(KeyCode.LeftShift) && fPlayerStaminaCounter > 0.0f) {
            velocity *= fSprintMultiplier;
            fPlayerStaminaCounter -= Time.deltaTime * fSprintDrain;
            if(fPlayerStaminaCounter < 0.0f) {
                fPlayerStaminaCounter = 0.0f;
            }
            // Set stamina regen delay
            bIsRegainingStamina = false;
            StartCoroutine(StaminaRegainDelay());
        }
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
        healthMeter.value = iLife;
        AudioController.PlayerPain();
        if(iLife <= 0 && !isDead) {
            isDead = true;
            anim.SetTrigger("Death");
            velocity = Vector3.zero;
        }
    }

    // Throws coins out as a distraction
    public void CreateCoinPileDistraction() {
        if(iCoinCount >= iCoinDistractionCost) {
            iCoinCount -= iCoinDistractionCost;
            GameObject distraction = Instantiate( Resources.Load("Coin Pile Distraction", typeof(GameObject))) as GameObject;
            distraction.transform.position = transform.position + Vector3.up* 0.2f + transform.forward * 0.1f;
            distraction.GetComponent<Rigidbody>().AddForce(_camera.transform.forward * 10, ForceMode.Impulse);
            // PlayerUIController.UpdateCoinText(iCoinCount);
            coinText.text = iCoinCount.ToString();
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
        iCoinCount += Mathf.FloorToInt(_iCoinValue * fCoinMultiplier);
        // Update game manager with player's new current coin count
        GameManager.PlayerCollectedCoins(iCoinCount);

        // Update text
        coinText.text = iCoinCount.ToString();
    }

    public static int PayTribute() {
        int temp = instance.iCoinCount;
        instance.iCoinCount = 0;
        instance.coinText.text = instance.iCoinCount.ToString();
        return temp;
    }

    private void Attack() {
        bIsAttacking = true;
        if(fPlayerStaminaCounter <= 0.0f) {
            return;
        }
        fPlayerStaminaCounter -= fStaminaDrainLight;
        if(fPlayerStaminaCounter < 0) {
            fPlayerStaminaCounter = 0;
        }
        // Stamina regain delay
        bIsRegainingStamina = false;
        StartCoroutine(StaminaRegainDelay());
        // Animation
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        string clipName = clipInfo[0].clip.name;

        // Cue animation triggers accordingly
        SwordCollider.enabled = true;
        anim.SetTrigger("Attack");
        SwordCollider.enabled = true;
        anim.SetTrigger("Attack02");
    }

    IEnumerator AttackCooldown(float _fAttackCooldown) {
        yield return new WaitForSeconds(_fAttackCooldown);
        bIsAttacking = false;
        SwordCollider.enabled = false;
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
        iLife = PlayerProgressionController.GetHealth();
        fPlayerToxicity = PlayerProgressionController.GetToxicity();
        fPlayerStamina = PlayerProgressionController.GetStamina();
        iTurnUndeadUses = PlayerProgressionController.GetTurnUndead();
        fDamageMod = PlayerProgressionController.GetDamage();
        fCoinMultiplier = PlayerProgressionController.GetCoinMultiplier();
    }

    private void InitialisePlayerUI() {
        // Find UI
        GameObject playerUI = GameObject.Find("InGameUI");
        if (playerUI == null) {
            Debug.LogError("ERROR: PlayerUI could not be found. Sliders could not be initialised.");
            return;
        }
        // Coin text
        coinText = playerUI.transform.Find("TextBackground/Text").GetComponent<Text>();
        // Toxicity meter
        toxicityMeter = playerUI.transform.Find("UndeadTimer/TimerBar").GetComponent<Image>();
        toxicityMeter.fillAmount = 0;
        // Health meter
        healthMeter = playerUI.transform.Find("HealthBar").GetComponent<Slider>();
        healthMeter.maxValue = iLife;
        healthMeter.value = iLife;
        // Stamina meter
        staminaMeter = playerUI.transform.Find("StaminaBar").GetComponent<Slider>();
        staminaMeter.maxValue = fPlayerStamina;
        staminaMeter.value = fPlayerStamina;
        fPlayerStaminaCounter = fPlayerStamina;
        // Turn Undead
        turnUndeadText = playerUI.transform.Find("Number of Undead").GetComponent<Text>();
        turnUndeadText.text = iTurnUndeadUses.ToString();
        // Compass
        compass = playerUI.transform.Find("Compass").GetComponent<Slider>();

    }

    // A small delay before the player can regain stamina
    private IEnumerator StaminaRegainDelay() {
        bIsRegainingStamina = false;
        yield return new WaitForSeconds(fStaminaRegainDelay);
        bIsRegainingStamina = true;
    }

    public static float GetAttackMultiplier() {
        return instance.fDamageMod;
    }

    public void DrainStamina(int _iDamage) {
        fPlayerStaminaCounter -= _iDamage;
        // Stamina regain delay
        bIsRegainingStamina = false;
        StartCoroutine(StaminaRegainDelay());
    }


    public static void SetCompassTarget(Transform target) {
        instance.compassTarget = target;
    }

    void UpdateCompass() {
        Vector3 toTarget = (compassTarget.position - transform.position).normalized;
        toTarget.y = 0;
        float dot = Vector3.Dot(transform.forward, toTarget);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (toTarget.x - transform.forward.x < 0) {
            angle = -1 * angle;
        }
        compass.value = angle;
    }

    public static void ReturnToMainMenu() {
        SceneManager.LoadScene(0);
    }

}
