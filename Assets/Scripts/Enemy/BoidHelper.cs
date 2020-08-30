using UnityEngine;

/// <summary>
/// All credit goes to Sebastian Lague.
/// Source code found here: https://github.com/SebLague/Boids
/// </summary>
public static class BoidHelper
{

    public const int NUM_VIEW_DIRECTIONS = 300;
    public static readonly Vector3[] Directions;

    static BoidHelper()
    {
        Directions = new Vector3[NUM_VIEW_DIRECTIONS];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < NUM_VIEW_DIRECTIONS; i++)
        {
            float t = (float)i / NUM_VIEW_DIRECTIONS;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            Directions[i] = new Vector3(x, y, z);
        }
    }

}