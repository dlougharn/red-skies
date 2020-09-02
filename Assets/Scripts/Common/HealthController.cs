using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public bool IsPlayer;
    public float DamageOnHit = 1f;
    public float MaxHealth = 50;
    public float CurrentHealth = 0;
    public GameObject DeathParticleEffect;
    public string ExplosionSoundEvent = "Explosion";

    private bool _isDead = false;
    private string _damageTag;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        if (IsPlayer)
        {
            _damageTag = "EnemyProjectile";
        }
        else
        {
            _damageTag = "PlayerProjectile";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead)
        {
            return;
        }

        if (collision.gameObject.tag == _damageTag)
        {
            CurrentHealth -= DamageOnHit;
            if (CurrentHealth <= 0)
            {
                var particleEffect = Instantiate(DeathParticleEffect, transform.position, transform.rotation);
                _isDead = true;
                AkSoundEngine.PostEvent(ExplosionSoundEvent, gameObject);
                if (IsPlayer)
                {
                    AkSoundEngine.PostEvent("Dead", gameObject);
                }
                Destroy(particleEffect, 20f);
                Destroy(gameObject);
            }
        }
    }
}
