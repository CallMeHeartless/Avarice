using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    public int iLife = 180;
    public float fSpeed = 5.0f;
    public float lookSensitivity = 10.0f;
    public float fBleedInterval = 10.0f;
    public int iBleedAmount = 10;
    public int iTurnUndeadUses = 1;
    public int iCoinCount = 0;

    [SerializeField]
    private int iCoinDistractionCost = 50;

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
	}
	
	// Update is called once per frame
	void Update () {
		if(iLife <= 0) {
            return;
        }

        SetMovement();
        BleedPlayer();

        if(Input.GetKeyDown(KeyCode.Space)) { // Default key for now
            CreateCoinPileDistraction();
        }
        if(Input.GetKeyDown(KeyCode.T)) {
            TurnUndead();
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

        // Set rotation
        rotation = new Vector3(0.0f, Input.GetAxisRaw("Mouse X"), 0.0f) * lookSensitivity;

        // Camera rotation 
        cameraRotation = Input.GetAxisRaw("Mouse Y") * lookSensitivity;
    }

    // Damages the player's health in set intervals.
    private void BleedPlayer() {
        fBleedCounter += Time.deltaTime;
        if(fBleedCounter >= fBleedInterval) {
            fBleedCounter = 0.0f;
            iLife -= iBleedAmount;
            Debug.Log(iLife);
        }
    }

    public void DamagePlayer(int _iDamage) {
        iLife -= _iDamage;
        PlayerUIController.UpdateHealthSlider(iLife);
    }

    // Throws coins out as a distraction
    void CreateCoinPileDistraction() {
        if(iCoinCount >= iCoinDistractionCost) {
            iCoinCount -= iCoinDistractionCost;
            GameObject distraction = Instantiate( Resources.Load("Coin Pile Distraction", typeof(GameObject))) as GameObject;
            distraction.transform.position = transform.position + transform.forward;
            distraction.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
            PlayerUIController.UpdateCoinText(iCoinCount);
        }
    }

    // Turns undead, making enemies flee the player for a short time
    void TurnUndead() {
        if(iTurnUndeadUses > 0) {
            --iTurnUndeadUses;
            // Call static game manager function
            Debug.Log(iTurnUndeadUses);
        }
    }
}
