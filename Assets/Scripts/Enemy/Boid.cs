using UnityEngine;
using System.Collections;

/// <summary>
/// All credit goes to Sebastian Lague.
/// Source code found here: https://github.com/SebLague/Boids
/// </summary>
public class Boid : MonoBehaviour
{
    public int Id;

    // To update:
    [HideInInspector]
    public Vector3 AvgFlockHeading;
    [HideInInspector]
    public Vector3 AvgAvoidanceHeading;
    [HideInInspector]
    public Vector3 CentreOfFlockmates;
    [HideInInspector]
    public int NumPerceivedFlockmates;

    private BoidSettings _boidSettings;
    private Transform _target;
    private Vector3 _velocity;

    public void Initialize(BoidSettings boidSettings, Transform target, int id)
    {
        _boidSettings = boidSettings;
        _target = target;

        var startSpeed = (_boidSettings.MinSpeed + _boidSettings.MaxSpeed) / 2;
        _velocity = transform.forward * startSpeed;

        Id = id;
    }

    public void UpdateBoid()
    {
        var acceleration = Vector3.zero;

        if (_target != null)
        {
            var offsetToTarget = (_target.position - transform.position);
            acceleration = SteerTowards(offsetToTarget) * _boidSettings.TargetWeight * Vector3.Distance(_target.position, transform.position);
        }

        if (NumPerceivedFlockmates != 0)
        {
            CentreOfFlockmates /= NumPerceivedFlockmates;

            var offsetToFlockmatesCentre = (CentreOfFlockmates - transform.position);

            var alignmentForce = SteerTowards(AvgFlockHeading) * _boidSettings.AlignWeight;
            var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * _boidSettings.CohesionWeight;
            var seperationForce = SteerTowards(AvgAvoidanceHeading) * _boidSettings.SeperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        if (IsHeadingForCollision())
        {
            var collisionAvoidDir = ObstacleRays();
            var collisionAvoidForce = SteerTowards(collisionAvoidDir) * _boidSettings.AvoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        _velocity += acceleration * Time.deltaTime;
        var speed = _velocity.magnitude;
        var dir = _velocity / speed;
        speed = Mathf.Clamp(speed, _boidSettings.MinSpeed, _boidSettings.MaxSpeed);
        _velocity = dir * speed;

        transform.position += _velocity * Time.deltaTime;
        transform.forward = dir;
    }

    private bool IsHeadingForCollision()
    {
        return Physics.SphereCast(transform.position, _boidSettings.BoundsRadius, transform.forward, out var hit, _boidSettings.CollisionAvoidDst, _boidSettings.ObstacleMask);
    }

    private Vector3 ObstacleRays()
    {
        var rayDirections = BoidHelper.Directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            var dir = transform.TransformDirection(rayDirections[i]);
            var ray = new Ray(transform.position, dir);
            if (!Physics.SphereCast(ray, _boidSettings.BoundsRadius, _boidSettings.CollisionAvoidDst, _boidSettings.ObstacleMask))
            {
                return dir;
            }
        }

        return transform.forward;
    }

    private Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * _boidSettings.MaxSpeed - _velocity;
        return Vector3.ClampMagnitude(v, _boidSettings.MaxSteerForce);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
