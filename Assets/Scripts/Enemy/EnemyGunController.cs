using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunController))]
public class EnemyGunController : MonoBehaviour
{
    public Transform Target;
    public float ActivateGunThreshold = 0f;

    private GunController _gunController;
    private bool _isDisabled = true;
    private bool _gunEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        _gunController = GetComponent<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDisabled)
        {
            AkSoundEngine.PostEvent("Stop_Laser_Gun_Fire", gameObject);
            return;
        }

        if (Target == null)
        {
            _gunController.DisableGun();
            AkSoundEngine.PostEvent("Stop_Laser_Gun_Fire", gameObject);
            return;
        }

        var direction = Target.position - transform.position;
        if (!_gunEnabled && Vector3.Dot(direction, transform.forward) > ActivateGunThreshold)
        {
            _gunEnabled = true;
            _gunController.EnableGun();
            AkSoundEngine.PostEvent("Laser_Gun_Fire", gameObject);
        }
        else if (_gunEnabled && Vector3.Dot(direction, transform.forward) <= ActivateGunThreshold)
        {
            _gunEnabled = false;
            _gunController.DisableGun();
            AkSoundEngine.PostEvent("Stop_Laser_Gun_Fire", gameObject);
        }
    }

    public void DisableGun()
    {
        _isDisabled = true;
        _gunController.DisableGun();
    }

    public void EnableGun()
    {
        _isDisabled = false;
    }
}
