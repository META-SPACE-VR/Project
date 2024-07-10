using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : InteractiveObject
{
    public void Awake()
    {
        Type = ObjectType.Collectable;
    }
}
