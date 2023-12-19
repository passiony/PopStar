using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsUtil : MonoBehaviour
{
    public static RaycastHit2D Raycast(Vector3 origin, Vector3 dir, float checkDistance, string layerName)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, checkDistance, 1 << LayerMask.NameToLayer(layerName));
        return hit;
    }
}