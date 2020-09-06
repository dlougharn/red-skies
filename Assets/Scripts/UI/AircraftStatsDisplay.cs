using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AircraftStatsDisplay : MonoBehaviour
{
    public Image HealthBar;
    public Image BoostBar;

    private HealthController _healthController;
    private AircraftController _aircraftController;

    // Start is called before the first frame update
    void Start()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _healthController = playerObject.GetComponent<HealthController>();
            if (_healthController != null)
            {
                _healthController.OnDamageTaken += UpdateHealth;
            }

            _aircraftController = playerObject.GetComponent<AircraftController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BoostBar != null && _aircraftController != null)
        {
            BoostBar.fillAmount = _aircraftController.CurrentRocketFuel / _aircraftController.MaxRocketFuel;
        }
    }

    private void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (HealthBar != null)
        {
            HealthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
