using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(AircraftController))]
public class PlayerController : MonoBehaviour
{
    public float InitialSpeed;
    public float MouseInputBoxHeight;
    public float MouseInputBoxWidth;
    public float UnfoldTransitionTime;
    public CinemachineVirtualCamera VirtualCamera;
    public GameObject CameraTarget;

    private AircraftController _aircraftController;
    private Quaternion _cameraTargetRotation;
    private AircraftState _aircraftState = AircraftState.WingsUnfolded;

    // Start is called before the first frame update
    void Start()
    {
        _aircraftController = GetComponent<AircraftController>();
        _aircraftController.SetVelocity(transform.forward * InitialSpeed);
    }

    void FixedUpdate()
    {
        switch (_aircraftState)
        {
            case AircraftState.WingsUnfolded:
                UpdateWingsUnfoldedState();
                break;
            case AircraftState.WingsFolded:
                UpdateWingsFoldedState();
                break;
        }

        UpdateRocketBooster();

        Variables.PlayerAircraftTransform = transform;
    }

    private void UpdateWingsFoldedState()
    {
        CameraTarget.transform.rotation = _cameraTargetRotation;
    }

    private void UpdateWingsUnfoldedState()
    {
        _aircraftController.ApplyForwardForce();
        _aircraftController.ApplyPitch(GetVerticalInput());
        _aircraftController.ApplyRoll(GetHorizontalInput());

        if (Input.GetKey(KeyCode.W))
        {
            EnterFoldedWingState();
            _aircraftController.FlipForward();
        }
        if (Input.GetKey(KeyCode.S))
        {
            EnterFoldedWingState();
            _aircraftController.FlipBackward();
        }
        if (Input.GetKey(KeyCode.A))
        {
            EnterFoldedWingState();
            _aircraftController.FlipRight();
        }
        if (Input.GetKey(KeyCode.D))
        {
            EnterFoldedWingState();
            _aircraftController.FlipLeft();
        }
    }

    private void UpdateRocketBooster()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (_aircraftState == AircraftState.WingsFolded)
            {
                EnterUnfoldedWingState();
            }
            if (_aircraftState == AircraftState.WingsTransitioningToUnfolded)
            {
                _aircraftController.ApplyForwardForce();
            }
            _aircraftController.EnableRocket();
        }
        else
        {
            _aircraftController.DisableRocket();
        }
    }

    private void EnterFoldedWingState()
    {
        _aircraftController.FoldWings();
        _aircraftState = AircraftState.WingsFolded;
        VirtualCamera.gameObject.SetActive(false);
        _cameraTargetRotation = CameraTarget.transform.rotation;
    }

    private void EnterUnfoldedWingState()
    {
        _aircraftController.UnflodWings();
        CameraTarget.transform.rotation = transform.rotation;
        _aircraftState = AircraftState.WingsTransitioningToUnfolded;
        StartCoroutine(TransitionToUnfoldedWings());
        StartCoroutine(EnableCamera());
        VirtualCamera.gameObject.SetActive(true);
    }

    private IEnumerator TransitionToUnfoldedWings()
    {
        yield return new WaitForSeconds(UnfoldTransitionTime);
        _aircraftState = AircraftState.WingsUnfolded;
    }

    private IEnumerator EnableCamera()
    {
        yield return new WaitForSeconds(2);
        if (_aircraftState == AircraftState.WingsUnfolded)
        {
            VirtualCamera.gameObject.SetActive(true);
        }
    }

    private float GetVerticalInput()
    {
        var mouseMinPosition = Screen.height / 2 - MouseInputBoxHeight / 2;
        var mouseMaxPosition = Screen.height / 2 + MouseInputBoxHeight / 2;

        var normalizedMousePosition = Mathf.Clamp(
            Input.mousePosition.y,
            mouseMinPosition,
            mouseMaxPosition);

        return -Mathf.Lerp(-1f, 1f, (normalizedMousePosition - mouseMinPosition) / MouseInputBoxHeight);
    }

    private float GetHorizontalInput()
    {
        var mouseMinPosition = Screen.width / 2 - MouseInputBoxWidth / 2;
        var mouseMaxPosition = Screen.width / 2 + MouseInputBoxWidth / 2;

        var normalizedMousePosition = Mathf.Clamp(
            Input.mousePosition.x,
            mouseMinPosition,
            mouseMaxPosition);

        return Mathf.Lerp(-1f, 1f, (normalizedMousePosition - mouseMinPosition) / MouseInputBoxWidth);
    }

    private void OnDrawGizmos()
    {

        //Rect rect = new Rect(
        //    Screen.width / 2 - MouseInputBoxWidth / 2,
        //    Screen.height / 2 - MouseInputBoxHeight / 2,
        //    MouseInputBoxWidth,
        //    MouseInputBoxHeight);
        //UnityEditor.Handles.BeginGUI();
        //UnityEditor.Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.green);
        //UnityEditor.Handles.EndGUI();
    }
}
