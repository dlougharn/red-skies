using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySwarm : MonoBehaviour
{
    public BoidSettings EnemyBoidSettings;
    public List<Boid> Boids;
    public GameObject Target;

    // Use this for initialization
    void Start()
    {
        foreach (var boid in Boids)
        {
            boid.Initialize(EnemyBoidSettings, Target.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var boidA in Boids)
        {
            foreach (var boidB in Boids)
            {
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

        foreach (var boid in Boids)
        {
            boid.UpdateBoid();
        }
    }
}
