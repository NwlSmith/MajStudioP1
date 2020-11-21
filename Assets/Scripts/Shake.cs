using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{

    private Vector3 initPosition;
    public int shakeModifier = 0;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;

        StartCoroutine(ShakeEnum());
    }

    private IEnumerator ShakeEnum()
    {
        while (true)
        {
            transform.position = initPosition + new Vector3(Random.Range(-shakeModifier, shakeModifier), Random.Range(-shakeModifier, shakeModifier), Random.Range(-shakeModifier, shakeModifier));
            yield return new WaitForSeconds(.1f);
        }
    }
}
