using System;
using UnityEngine;

public class TentacleSoundDetector : MonoBehaviour
{

    private TentacleSoundPlayer _tentacleSoundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _tentacleSoundPlayer = GetComponentInParent<TentacleSoundPlayer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Hand"))
            _tentacleSoundPlayer.PlaySound();
    }
}
