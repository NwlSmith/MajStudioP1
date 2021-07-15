using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRChairRotator : MonoBehaviour
{
    
    public GameObject chair;
    private Camera cam;
    private float lerpValue = .02f;
    private float lerpValueMax = .05f;
    private float lerpValueMin = .02f;
    private float lerpValueTarget;
    private float lerpValueSmoothing = .05f;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRot = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        Quaternion currentRot = chair.transform.rotation;
        if (Quaternion.Angle(targetRot, currentRot) > 120f)
        {
            lerpValueTarget = lerpValueMax;
        }
        else if (Quaternion.Angle(targetRot, currentRot) > 90f)
        {
            lerpValueTarget = lerpValueMin;
        }

        lerpValue = Mathf.Lerp(lerpValue, lerpValueTarget, lerpValueSmoothing);
        chair.transform.rotation = Quaternion.Lerp(chair.transform.rotation, targetRot, lerpValue);
    }
}
