using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(LineRenderer))]
public class Bezier : MonoBehaviour {
    public bool endPointDetected;

    // Getters and Setters
    public Vector3 EndPoint {
        get { return endpoint; }
    }
    private Vector3 endpoint;

    public float ExtensionFactor {
        set { extensionFactor = value; }
    }
    private float extensionFactor;

    private Vector3[] controlPoints;
    private LineRenderer lineRenderer;
    private float extendStep;
    private int SEGMENT_COUNT = 50;

    void Start() {
        controlPoints = new Vector3[3];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        extendStep = 5f;
        extensionFactor = 0f;
    }
    void Update() {
        UpdateControlPoints();
        HandleExtension();
        DrawCurve();
    }

    public void ToggleDraw(bool draw) {
        lineRenderer.enabled = draw;
    }

    void HandleExtension() {
        if (extensionFactor == 0f)
            return;

        float finalExtension = extendStep + Time.deltaTime * extensionFactor;
        extendStep = Mathf.Clamp(finalExtension, 3.5f, 6.5f);
    }

    void UpdateControlPoints() {
        controlPoints[0] = gameObject.transform.position; // Get Controller Position
        controlPoints[1] = controlPoints[0] + (gameObject.transform.forward * extendStep * 2f / 5f);
        controlPoints[2] = controlPoints[1] + (gameObject.transform.forward * extendStep * 3f / 5f) + Vector3.up * -1f;
    }

    void DrawCurve() {
        if (!lineRenderer.enabled)
            return;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, controlPoints[0]);

        Vector3 prevPosition = controlPoints[0];
        Vector3 nextPosition = prevPosition;
        for (int i = 1; i <= SEGMENT_COUNT; i++) {
            float t = i / (float) SEGMENT_COUNT;
            lineRenderer.positionCount = i + 1;

            if (i == SEGMENT_COUNT) {
                Vector3 endDirection = Vector3.Normalize(prevPosition - lineRenderer.GetPosition(i-2));
                nextPosition = prevPosition + endDirection * 2f;
            } else {
                nextPosition = CalculateBezierPoint(t, controlPoints[0], controlPoints[1], controlPoints[2]);
            }

            if (CheckColliderIntersection(prevPosition, nextPosition)) {
                lineRenderer.SetPosition(i, endpoint);
                endPointDetected = true;
                return;
            }
            endPointDetected = false;
            lineRenderer.SetPosition(i, nextPosition);
            prevPosition = nextPosition;
        }
    }

    bool CheckColliderIntersection(Vector3 start, Vector3 end) {
        Ray r = new Ray(start, end - start);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, Vector3.Distance(start, end))) {
            endpoint = hit.point;
            return true;
        }

        return false;
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
        return 
            Mathf.Pow((1f - t), 2) * p0 +
            2f * (1f - t) * t * p1 +
            Mathf.Pow(t, 2) * p2;
    }
}
