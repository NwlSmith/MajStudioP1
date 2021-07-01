using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRChairRotator : MonoBehaviour
{
    
    public GameObject chair;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRot = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
        chair.transform.rotation = Quaternion.Lerp(chair.transform.rotation, targetRot, 0.2f);
    }
}
