using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTeleportPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(InitiateAnimation());
	}

    private void OnEnable() {
        StartCoroutine(InitiateAnimation());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private IEnumerator InitiateAnimation() {
        gameObject.transform.localScale = Vector3.zero;
        float targetScale = 0.5f;
        float currScale = 0f;
        Vector3 scaleVec = new Vector3(0, 0, 0);

        while (currScale < targetScale) {
            yield return 0;
            currScale += (Time.deltaTime * 0.5f);
            scaleVec.x = scaleVec.y = scaleVec.z = currScale;
            Debug.Log(currScale);
            gameObject.transform.localScale = scaleVec;
        }
        StartCoroutine(Oscillate());
    }

    private IEnumerator Oscillate() {
        float startTime = Time.time;
        Vector3 scaleVec = gameObject.transform.localScale;
        float scaleLimit = 0.1f;

        while (true) {
            float factor = Mathf.Sin( (Time.time - startTime) * 2f);
            scaleVec.x = scaleVec.y = scaleVec.z = (0.5f + factor * scaleLimit);
            yield return 0;
            gameObject.transform.localScale = scaleVec;
        }
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
