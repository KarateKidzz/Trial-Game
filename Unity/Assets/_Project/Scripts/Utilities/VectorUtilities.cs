using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtilities
{
    public static Vector3 ReplaceZ(this Vector3 original, float z)
    {
        return new Vector3(original.x, original.y, z);
    }
}
