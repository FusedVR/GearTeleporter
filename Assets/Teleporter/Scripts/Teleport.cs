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

	void Start () {
        teleportEnabled = false;
        firstClick = false;
        firstClickTime = 0f;
        teleportSprite.SetActive(false);
    }
	
	void Update () {
        UpdateTeleportEnabled();

        if (teleportEnabled) {
            HandleBezier();
            HandleTeleport();
        }
    }

    // On double-click, toggle teleport mode on and off.
    void UpdateTeleportEnabled() {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) { // The trigger is pressed.
            if (!firstClick) { // The first click is detected.
                firstClick = true;
                firstClickTime = Time.unscaledTime;
            } else { // The second click detected, so toggle teleport mode.
                firstClick = false;
                ToggleTeleportMode();
            }
        }

        if (Time.unscaledTime - firstClickTime > doubleClickTimeLimit) { // Time for the double click has run out.
            firstClick = false;
        }
    }

    void HandleTeleport() {
        if (bezier.endPointDetected) { // There is a point to teleport to.
            // Display the teleport point.
            teleportSprite.SetActive(true);
            teleportSprite.transform.position = bezier.EndPoint;

            if (OVRInput.GetDown(OVRInput.Button.One)) // Teleport to the position.
                TeleportToPosition(bezier.EndPoint);
        } else {
            teleportSprite.SetActive(false);
        }
    }

    void TeleportToPosition(Vector3 teleportPos) {
        gameObject.transform.position = teleportPos + Vector3.up * 0.5f;
    }

    // Optional: use the touchpad to move the teleport point closer or further.
    void HandleBezier() {
        Vector2 touchCoords = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        if (Mathf.Abs(touchCoords.y) > 0.8f) {
            bezier.ExtensionFactor = touchCoords.y > 0f ? 1f : -1f;
        } else {
            bezier.ExtensionFactor = 0f;
        }
    }

    void ToggleTeleportMode() {
        teleportEnabled = !teleportEnabled;
        bezier.ToggleDraw(teleportEnabled);
        if (!teleportEnabled) {
            teleportSprite.SetActive(false);
        }
    }
}
