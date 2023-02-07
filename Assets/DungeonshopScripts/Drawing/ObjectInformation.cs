using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInformation : MonoBehaviour
{
    public float size;

    public void updatePosition(Vector3 position)
    {
        transform.position = position;
        transform.GetChild(0).position = position;
    }
}
