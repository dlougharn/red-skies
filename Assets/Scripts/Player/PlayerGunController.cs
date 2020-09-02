using UnityEngine;
using System.Collections;

public class PlayerGunController : MonoBehaviour
{
    private GunController _gunController;
    private bool _gunEnabled = false;

    // Use this for initialization
    void Start()
    {
        _gunController = GetComponent<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !_gunEnabled)
        {
            _gunEnabled = true;
            _gunController.EnableGun();
            AkSoundEngine.PostEvent("Gun_Fire", gameObject);
        }
        else if (!Input.GetMouseButton(0) && _gunEnabled)
        {
            _gunEnabled = false;
            _gunController.DisableGun();
            AkSoundEngine.PostEvent("Stop_Gun_Fire", gameObject);
        }
    }
}
