using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public bool TeleportEnabled {
        get { return teleportEnabled; }
    }

    public Bezier bezier;
    public GameObject teleportSprite;

    private bool teleportEnabled;

    private bool firstClick;
    private float firstClickTime;
    private float doubleClickTimeLimit = 0.5f;

	// Use this for initialization
	void Start () {
        teleportEnabled = false;
        firstClick = false;
        firstClickTime = 0f;
        teleportSprite.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateTeleportEnabled();

        if (teleportEnabled) {
            HandleBezier();
            HandleTeleport();
        }
    }

    void UpdateTeleportEnabled() {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) { // The trigger is pressed.
            if (!firstClick) { // First click detected
                firstClick = true;
                firstClickTime = Time.unscaledTime;
            } else { // Second click detected.
                firstClick = false;
                ToggleTeleport();
            }
        }

        if (Time.unscaledTime - firstClickTime > doubleClickTimeLimit) {
            firstClick = false;
        }
    }

    void HandleTeleport() {
        if (bezier.endPointDetected) {
            teleportSprite.SetActive(true);
            teleportSprite.transform.position = bezier.EndPoint;
            if (OVRInput.GetDown(OVRInput.Button.One)) {
                Vector3 pos = bezier.EndPoint;
                TeleportToPosition(pos);
            }
        } else {
            teleportSprite.SetActive(false);
        }
    }

    void TeleportToPosition(Vector3 teleportPos) {
        gameObject.transform.position = teleportPos + Vector3.up;
    }

    void HandleBezier() {
        Vector2 touchCoords = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        if (Mathf.Abs(touchCoords.y) > 0.8f) {
            if (touchCoords.y > 0f) {
                bezier.ExtensionFactor = 1f;
            } else {
                bezier.ExtensionFactor = -1f;
            }
        } else {
            bezier.ExtensionFactor = 0f;
        }
    }

    void ToggleTeleport() {
        teleportEnabled = !teleportEnabled;
        bezier.ToggleDraw(teleportEnabled);
    }
}
