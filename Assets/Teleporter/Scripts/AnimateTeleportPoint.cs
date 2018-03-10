using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTeleportPoint : MonoBehaviour {
    public Material m;
    public Shader overlayShader;

    private float baseScale = 0.25f;
    private float oscillateFactor = 0.1f;

	void Start () {
        m.shader = overlayShader;
        StartCoroutine(InitiateAnimation());
	}

    private void OnEnable() {
        StartCoroutine(InitiateAnimation());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    // Scale up the point to the target scale.
    private IEnumerator InitiateAnimation() { 
        gameObject.transform.localScale = Vector3.zero;
        float currScale = 0f;
        Vector3 scaleVec = new Vector3(0, 0, 0);

        while (currScale < baseScale) {
            yield return 0;
            currScale += (Time.deltaTime * 0.5f);
            scaleVec.x = scaleVec.y = scaleVec.z = currScale;
            Debug.Log(currScale);
            gameObject.transform.localScale = scaleVec;
        }
        StartCoroutine(Oscillate());
    }

    // Oscillate the size of the teleport point.
    private IEnumerator Oscillate() {
        float startTime = Time.time;
        Vector3 scaleVec = gameObject.transform.localScale;

        while (true) {
            float factor = Mathf.Sin( (Time.time - startTime) * 2f);
            scaleVec.x = scaleVec.y = scaleVec.z = (baseScale + factor * oscillateFactor);
            yield return 0;
            gameObject.transform.localScale = scaleVec;
        } 
    }
}
