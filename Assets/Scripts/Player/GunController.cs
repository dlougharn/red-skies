using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public string ProjectileTag;
    public List<Transform> ProjectileSpawnLocations;
    public float ShootForce = 1000f;
    public float FireRate = 3f;
    public float ProjectileLifetime = 10f;
    public float BulletSpread = 0;

    private bool _gunEnabled = false;
    private float _timeSinceLastShot = 10000f;
    private float _timeBetweenShots = 1f;
    private int _projectileSpawnIndex = 0;
    private Rigidbody _rigidbody;
    private ObjectPooler _objectPooler;

    // Start is called before the first frame update
    void Start()
    {
        _timeBetweenShots = 1f / FireRate;
        _rigidbody = GetComponent<Rigidbody>();
        _objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        _timeSinceLastShot += Time.deltaTime;
        if (_timeSinceLastShot >= _timeBetweenShots && _gunEnabled)
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
        var projectile = _objectPooler.SpawnFromPool(ProjectileTag, ProjectileSpawnLocations[_projectileSpawnIndex].position, transform.rotation);
        var rigidBody = projectile.GetComponent<Rigidbody>();
        if (_rigidbody != null)
        {
            rigidBody.velocity += _rigidbody.velocity;
        }

        var projectileDirection = new Vector3(
            projectile.transform.forward.x + Random.Range(-BulletSpread, BulletSpread),
            projectile.transform.forward.y + Random.Range(-BulletSpread, BulletSpread),
            projectile.transform.forward.z + Random.Range(-BulletSpread, BulletSpread));

        rigidBody.AddForce(projectileDirection * ShootForce);

        RemoveProjectile(projectile, ProjectileLifetime);
    }

    private IEnumerator RemoveProjectile(GameObject projectile, float time)
    {
        yield return new WaitForSeconds(time);
        projectile.SetActive(false);
    }

    public void EnableGun()
    {
        _gunEnabled = true;
    }

    public void DisableGun()
    {
        _gunEnabled = false;
    }
}
