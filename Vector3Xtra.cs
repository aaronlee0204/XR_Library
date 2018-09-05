using UnityEngine;

//Extension methods must be defined in a static class
public static class Vector3Xtra  {

    // This method extends Unity's Vector3 class to provide the missing scale inverse operator.
    // Invert a scale vector (private).
    static Vector3 ScaleInverse(this Vector3 A, Vector3 scale)
    {
        Vector3 invscale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
        return invscale;
    }

 }
