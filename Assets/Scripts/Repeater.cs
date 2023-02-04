using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : MonoBehaviour
{

    public int reachRepeater;
    public List<Rail> repeatedRails;
    public Vector3Int gridPosition;

    void Start()
    {
        gridPosition = GPCtrl.Instance.railMap.WorldToCell(transform.position);
        DetectRails();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            DetectRails();
        }
    }

    public void DetectRails()
    {
        repeatedRails.Clear();
        foreach (Rail x in GPCtrl.Instance.rails)
        {
            float _distance = Mathf.Sqrt(Mathf.Pow((x.gridPosition.x - gridPosition.x), 2)) + Mathf.Sqrt(Mathf.Pow((x.gridPosition.y - gridPosition.y), 2));
            if (_distance <= reachRepeater)
            {
                repeatedRails.Add(x);
                x.ActivateRail();
            }
        }
    }
}
