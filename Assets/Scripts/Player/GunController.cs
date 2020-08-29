using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject ProjectilePrefab;
    public List<Transform> ProjectileSpawnLocations;
    public float ShootForce = 1000f;
    public float FireRate = 3f;
    public float ProjectileLifetime = 10f;

    private float _timeSinceLastShot = 10000f;
    private float _timeBetweenShots = 1f;
    private int _projectileSpawnIndex = 0;
    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _timeBetweenShots = 1f / FireRate;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
        if (_timeSinceLastShot >= _timeBetweenShots && Input.GetMouseButton(0))
        {
            UpdateSpawnIndex();
            FireProjectile();
            _timeSinceLastShot = 0f;
        }
    }

    private void UpdateSpawnIndex()
    {
        _projectileSpawnIndex = (_projectileSpawnIndex + 1) % ProjectileSpawnLocations.Count;
    }

    private void FireProjectile()
    {
        var projectile = Instantiate(ProjectilePrefab);
        projectile.transform.position = ProjectileSpawnLocations[_projectileSpawnIndex].position;
        projectile.transform.rotation = transform.rotation;
        var rigidBody = projectile.GetComponent<Rigidbody>();
        rigidBody.velocity += _rigidbody.velocity;
        rigidBody.AddForce(projectile.transform.forward * ShootForce);

        Destroy(projectile, ProjectileLifetime);
    }
}
