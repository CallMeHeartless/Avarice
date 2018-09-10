using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    public int iLife = 180;
    public float fSpeed = 5.0f;
    public float lookSensitivity = 10.0f;

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotation = 0.0f;
    private float currentCameraRotation = 0.0f;
    private Camera _camera;

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

        // Set movement
        Vector3 forwardMovement = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 sideMovement = transform.right * Input.GetAxisRaw("Horizontal");

        velocity = (forwardMovement + sideMovement).normalized * fSpeed;

        // Set rotation
        rotation = new Vector3(0.0f, Input.GetAxisRaw("Mouse X"), 0.0f) * lookSensitivity;

        // Camera rotation 
        cameraRotation = Input.GetAxisRaw("Mouse Y") * lookSensitivity;

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
}
