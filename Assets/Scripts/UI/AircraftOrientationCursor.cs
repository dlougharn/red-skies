using UnityEngine;
using System.Collections;

public class AircraftOrientationCursor : MonoBehaviour
{
    public float CrossHairDistance = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Variables.PlayerAircraftTransform != null)
        {
            RotateCursor();
            PositionCursor();
        }
    }

    private void RotateCursor()
    {
        var angle = Variables.PlayerAircraftTransform.rotation.eulerAngles.z - Camera.main.transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Da
    }

    private void PositionCursor()
    {
        var currentVelocity = Vector3.zero;
        transform.position = Camera.main.WorldToScreenPoint(Variables.PlayerAircraftTransform.TransformPoint(Vector3.forward * CrossHairDistance));
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.05f);
    }

    private void OnDrawGizmos()
    {
        if (Variables.PlayerAircraftTransform != null)
        {
            Gizmos.DrawSphere(Variables.PlayerAircraftTransform.TransformPoint(Vector3.forward * CrossHairDistance), 1f);
        }
    }
}
