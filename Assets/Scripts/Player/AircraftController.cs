using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AircraftController : MonoBehaviour
{

    public float EngineThrustPower;
    public float RocketThrustPower;
    public float PitchPower;
    public float RollPower;
    public float YawPower;
    public float PitchFlipPower;
    public float YawFlipPower;
    public float DebugRayMultiplier = 0.0001f;
    public float WingSpan = 13.56f;
    public float WingArea = 78.04f;
    public Rigidbody Rigidbody;
    public GameObject RocketBoostEffect;

    private bool _wingsFolded = false;
    private float _aspectRatio;
    private bool _rocketEnabled = false;
    private float _originalAngularDrag;
    private float _originalLinearDrag;
    private Vector3 _liftVector;
    private Vector3 _dragVector;
    private Vector3 _gravityVector;
    private Vector3 _thrustVector;
    private AircraftAnimationController _aircraftAnimationController;

    private void Start()
    {
        _aspectRatio = (WingSpan * WingSpan) / WingArea;
        _originalAngularDrag = Rigidbody.angularDrag;
        _originalLinearDrag = Rigidbody.drag;
        RocketBoostEffect.SetActive(false);
        _aircraftAnimationController = GetComponent<AircraftAnimationController>();
    }

    private void FixedUpdate()
    {
        if (!_wingsFolded)
        {
            CalculateForces();
        }
        if (_rocketEnabled)
        {
            Rigidbody.AddForce(transform.forward * RocketThrustPower);
        }
    }

    //Source: https://stackoverflow.com/questions/49716989/unity-aircraft-physics
    private void CalculateForces()
    {
        // *flip sign(s) if necessary*
        var localVelocity = transform.InverseTransformDirection(Rigidbody.velocity);
        var angleOfAttack = Mathf.Atan2(-localVelocity.y, localVelocity.z);

        // α * 2 * PI * (AR / AR + 2)
        var inducedLift = angleOfAttack * (_aspectRatio / (_aspectRatio + 2f)) * 2f * Mathf.PI;

        // CL ^ 2 / (AR * PI)
        var inducedDrag = (inducedLift * inducedLift) / (_aspectRatio * Mathf.PI);

        // V ^ 2 * R * 0.5 * A
        var pressure = Rigidbody.velocity.sqrMagnitude * 1.2754f * 0.5f * WingArea;

        var lift = inducedLift * pressure;
        var drag = (0.021f + inducedDrag) * pressure;

        // *flip sign(s) if necessary*
        var dragDirection = Rigidbody.velocity.normalized;
        var liftDirection = Vector3.Cross(dragDirection, transform.right);

        _liftVector = liftDirection * lift;
        _dragVector = dragDirection * drag;

        // Lift + Drag = Total Force
        Rigidbody.AddForce(_liftVector - _dragVector);

        // Gravity
        _gravityVector = Vector3.down * Constants.GRAVITY * Rigidbody.mass;
        Rigidbody.AddForce(_gravityVector);
    }

    /// <summary>
    /// Apply forward force to the aircraft. 
    /// Amount of force determines by <see cref="AircraftController.EngineThrustPower"/>.
    /// </summary>
    public void ApplyForwardForce()
    {
        _thrustVector = transform.forward * EngineThrustPower;
        Rigidbody.AddForce(_thrustVector);
    }

    public void FlipForward()
    {
        ApplyPitch(-PitchFlipPower);
    }

    public void FlipBackward()
    {
        ApplyPitch(PitchFlipPower);
    }

    public void FlipLeft()
    {
        ApplyYaw(-YawFlipPower);
    }

    public void FlipRight()
    {
        ApplyYaw(YawFlipPower);
    }

    /// <summary>
    /// Apply pitch to the aircraft using torque.
    /// </summary>
    /// <param name="pitchDirection">Float between -1 and 1 indicating the pitch direction.</param>
    public void ApplyPitch(float pitchDirection)
    {
        Rigidbody.AddTorque(transform.right * PitchPower * pitchDirection);
        _aircraftAnimationController.ShowPitch(pitchDirection);
    }

    /// <summary>
    /// Apply roll to the aircraft using torque.
    /// </summary>
    /// <param name="rollDirection">Float between -1 and 1 indicating the roll direction.</param>
    public void ApplyRoll(float rollDirection)
    {
        Rigidbody.AddTorque(transform.forward * RollPower * -rollDirection);
        _aircraftAnimationController.ShowRoll(rollDirection);
    }

    /// <summary>
    /// Apply roll to the aircraft using torque.
    /// </summary>
    /// <param name="yawDirection">Float between -1 and 1 indicating the roll direction.</param>
    public void ApplyYaw(float yawDirection)
    {
        Rigidbody.AddTorque(transform.up * YawPower * -yawDirection);
    }

    /// <summary>
    /// Sets the velocity of the aircraft rigid body.
    /// </summary>
    /// <param name="velocity">The velocity to set.</param>
    public void SetVelocity(Vector3 velocity)
    {
        Rigidbody.velocity = velocity;
    }

    /// <summary>
    /// Enables the rocket booster.
    /// </summary>
    public void EnableRocket()
    {
        RocketBoostEffect.SetActive(true);
        _rocketEnabled = true;
    }

    /// <summary>
    /// Disables the rocket booster.
    /// </summary>
    public void DisableRocket()
    {
        RocketBoostEffect.SetActive(false);
        _rocketEnabled = false;
    }


    public void FoldWings()
    {
        _wingsFolded = true;
        Rigidbody.angularDrag = 0;
        Rigidbody.drag = 0;
        _aircraftAnimationController.FoldWings();
    }

    public void UnflodWings()
    {
        _wingsFolded = false;
        Rigidbody.angularDrag = _originalAngularDrag;
        Rigidbody.drag = _originalLinearDrag;
        _aircraftAnimationController.UnfoldWings();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _liftVector * DebugRayMultiplier);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _gravityVector * DebugRayMultiplier);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _thrustVector * DebugRayMultiplier);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, _dragVector * DebugRayMultiplier);
    }
}