using UnityEngine;
using System.Collections;

public class OnProjectileHit : MonoBehaviour
{
    public GameObject WaterParticleEffectPrefab;
    public GameObject AircraftHitParticleEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            var particleEffect = Instantiate(WaterParticleEffectPrefab, transform.position, transform.rotation);
            Destroy(particleEffect, 5f);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            var particleEffect = Instantiate(AircraftHitParticleEffectPrefab, transform.position, transform.rotation);
            Destroy(particleEffect, 5f);

            if (collision.gameObject.tag == "Player")
            {
                AkSoundEngine.PostEvent("Bullet_Hit_Metal", collision.gameObject);
            }
        }
    }
}
