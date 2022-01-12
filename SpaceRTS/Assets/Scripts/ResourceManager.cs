using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{
    private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
    public static Vector3 InvalidPosition { get { return invalidPosition; } }
}
