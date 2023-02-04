using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{

    public bool isActivated = false;
    public Vector3Int gridPosition;
    void Start()
    {
        //gridPosition = new Vector3Int(Mathf.RoundToInt(transform.position.x), (int)(transform.position.y), Mathf.RoundToInt(transform.position.y));
        //Debug.Log("new rail grid position : " + (int)(transform.position.y + 0.0001f));
    }

    void Update()
    {
        
    }

    public bool CheckIfRailActivated()
    {
        return isActivated;
    }
}
