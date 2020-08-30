using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float DamageOnHit = 1f;
    public float MaxHealth = 50;
    public GameObject DeathParticleEffect;

    private float _currentHealth = 0;
    private bool _isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead)
        {
            return;
        }

        if (collision.gameObject.tag == "Projectile")
        {
            _currentHealth -= DamageOnHit;
            if (_currentHealth <= 0)
            {
                Instantiate(DeathParticleEffect, transform.position, transform.rotation);
                _isDead = true;
                Destroy(gameObject);
            }
        }
    }
}
