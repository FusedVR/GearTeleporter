using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLine : MonoBehaviour {
    public Material m;

    private Vector2 uvAnimationRate = new Vector2(-2.0f, 0.0f);
    private string textureName = "_MainTex";


    Vector2 uvOffset = Vector2.zero;
    void LateUpdate() {
        uvOffset += (Time.deltaTime * uvAnimationRate);
        m.SetTextureOffset(textureName, uvOffset);
    }
}
