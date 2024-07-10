using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomableObject : InteractiveObject
{
    public void Awake()
    {
        Type = ObjectType.Zoomable;
    }
}
