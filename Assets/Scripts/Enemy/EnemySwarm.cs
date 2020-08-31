using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySwarm : MonoBehaviour
{
    public BoidSettings EnemyBoidSettings;
    public Transform PlayerTarget;
    public HealthController MotherShip;
    public float TargetSwitchDistance = 1000f;

    private List<Boid> _boids;
    private float _initialMotherShipHealth;
    private bool _isTargetingPlayer = false;

    // Use this for initialization
    void Start()
    {
        _initialMotherShipHealth = MotherShip.MaxHealth;
        _boids = new List<Boid>();
        foreach (Transform child in transform)
        {
            var boid = child.GetComponent<Boid>();
            if (boid != null)
            {
                boid.Initialize(EnemyBoidSettings, MotherShip.transform);
                _boids.Add(boid);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < _boids.Count; i++)
        {
            if (_boids[i] == null)
            {
                _boids.RemoveAt(i);
                i--;
            }
        }

        foreach (var boidA in _boids)
        {
            //Boid was destroyed
            if (boidA == null)
            {
                continue;
            }

            foreach (var boidB in _boids)
            {
                if (boidB == null)
                {
                    continue;
                }

                if (boidA != boidB)
                {
                    var offset = boidB.transform.position - boidA.transform.position;
                    var sqrDst = offset.x * offset.x + offset.y * offset.y + offset.z * offset.z;

                    if (sqrDst < EnemyBoidSettings.PerceptionRadius * EnemyBoidSettings.PerceptionRadius)
                    {
                        boidA.NumPerceivedFlockmates += 1;
                        boidA.AvgFlockHeading += boidB.transform.forward;
                        boidA.CentreOfFlockmates += boidB.transform.position;

                        if (sqrDst < EnemyBoidSettings.AvoidanceRadius * EnemyBoidSettings.AvoidanceRadius)
                        {
                            boidA.AvgAvoidanceHeading -= offset / sqrDst;
                        }
                    }
                }
            }
        }

        var target = GetBoidTarget();
        foreach (var boid in _boids)
        {
            boid.SetTarget(target);
            boid.UpdateBoid();
            var gunController = boid.GetComponent<EnemyGunController>();
            if (_isTargetingPlayer)
            {
                gunController.EnableGun();
            }
            else
            {
                gunController.DisableGun();
            }
        }
    }

    private Transform GetBoidTarget()
    {
        if (MotherShip == null)
        {
            return PlayerTarget;
        }
        if (PlayerTarget == null)
        {
            return MotherShip?.transform;
        }

        if (_isTargetingPlayer)
        {
            if (Vector3.Distance(MotherShip.transform.position, PlayerTarget.transform.position) > TargetSwitchDistance)
            {
                _isTargetingPlayer = false;
                return MotherShip.transform;
            }
            else
            {
                return PlayerTarget;
            }
        }

        if (MotherShip.CurrentHealth < _initialMotherShipHealth)
        {
            _isTargetingPlayer = true;
            _initialMotherShipHealth = MotherShip.CurrentHealth;
            return PlayerTarget;
        }

        return MotherShip.transform;
    }
}
