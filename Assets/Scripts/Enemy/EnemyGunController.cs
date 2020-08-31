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
            return;
        }

        if (Target == null)
        {
            _gunController.DisableGun();
            return;
        }

        var direction = Target.position - transform.position;
        if (Vector3.Dot(direction, transform.forward) > ActivateGunThreshold)
        {
            _gunController.EnableGun();
        }
        else
        {
            _gunController.DisableGun();
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
