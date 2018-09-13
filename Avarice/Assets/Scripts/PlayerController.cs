using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    public static bool bIsAttacking = false;
    public int iLife = 180;
    public float fSpeed = 5.0f;
    public float flookSensitivity = 10.0f;
    public float fAttackRate = 0.6f;
    public float fBleedInterval = 10.0f;
    public int iBleedAmount = 10;
    public int iTurnUndeadUses = 1;
    public float fTurnDuration = 10.0f;
    public int iCoinCount = 0;

    [SerializeField]
    private int iCoinDistractionCost = 50;
    //[SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform coinThrow;

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
        PlayerUIController.SetHealthSliderMaxValue(iLife);
        
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
        } else {
            anim.SetTrigger("Idle");
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
            //anim.SetTrigger("LiftSword");
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

}
