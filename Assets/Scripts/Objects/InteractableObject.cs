using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum ObjectType
    {
        None = 0,
        Rail = 1,
        Repeater = 2,
        Crystal = 3,
        Spawner = 4
    }

    public ObjectType objectType;
    public Vector3Int gridPosition;


    void Start()
    {
        gridPosition = GPCtrl.Instance.railMap.WorldToCell(transform.position);
    }
}
