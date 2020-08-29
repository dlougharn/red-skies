using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftAnimationController : MonoBehaviour
{
    public GameObject Propeller;
    public float PropellerSpinSpeed;
    public GameObject LeftFlap;
    public GameObject RightFlap;
    public float MaxFlapAngle = 40f;
    public GameObject LeftElevator;
    public GameObject RightElevator;
    public float MaxElevatorAngle = 40f;
    public GameObject LeftWing;
    public GameObject RightWing;
    public float WingFoldAngle = -50f;
    public float WingFoldSpeed = 100f;

    private Quaternion _targetRightWingRotation;
    private Quaternion _targetLeftWingRotation;

    // Start is called before the first frame update
    void Start()
    {
        _targetRightWingRotation = Quaternion.Euler(0, 0, 0);
        _targetLeftWingRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Propeller.transform.Rotate(0, PropellerSpinSpeed * Time.deltaTime, 0);
        LeftWing.transform.localRotation = Quaternion.RotateTowards(LeftWing.transform.localRotation, _targetLeftWingRotation, Time.deltaTime * WingFoldSpeed);
        RightWing.transform.localRotation = Quaternion.RotateTowards(RightWing.transform.localRotation, _targetRightWingRotation, Time.deltaTime * WingFoldSpeed);
    }

    public void ShowRoll(float roll)
    {
        roll = Mathf.Clamp(roll, -1f, 1f);
        LeftFlap.transform.localRotation =
            Quaternion.Euler(-MaxFlapAngle * roll, LeftFlap.transform.localRotation.eulerAngles.y, LeftFlap.transform.localRotation.eulerAngles.z);
        RightFlap.transform.localRotation =
            Quaternion.Euler(MaxFlapAngle * roll, RightFlap.transform.localRotation.eulerAngles.y, RightFlap.transform.localRotation.eulerAngles.z);

        //Leave this here in case I decide to tackle Unity rotation axis order issue.
        //LeftFlap.transform.localRotation = Quaternion.AngleAxis(30 * roll, Vector3.right) *
        //    Quaternion.AngleAxis(LeftFlap.transform.localRotation.eulerAngles.z, Vector3.forward) *
        //    Quaternion.AngleAxis(LeftFlap.transform.localRotation.eulerAngles.y, Vector3.up);
    }

    public void ShowPitch(float pitch)
    {
        pitch = Mathf.Clamp(pitch, -1f, 1f);
        LeftElevator.transform.localRotation =
            Quaternion.Euler(-MaxElevatorAngle * pitch, LeftElevator.transform.localRotation.eulerAngles.y, LeftElevator.transform.localRotation.eulerAngles.z);
        RightElevator.transform.localRotation =
            Quaternion.Euler(MaxElevatorAngle * pitch, RightElevator.transform.localRotation.eulerAngles.y, RightElevator.transform.localRotation.eulerAngles.z);
    }

    public void FoldWings()
    {
        _targetRightWingRotation = Quaternion.Euler(0, WingFoldAngle, 0);
        _targetLeftWingRotation = Quaternion.Euler(0, -WingFoldAngle, 0);
    }

    public void UnfoldWings()
    {
        _targetRightWingRotation = Quaternion.Euler(0, 0, 0);
        _targetLeftWingRotation = Quaternion.Euler(0, 0, 0);
    }
}
