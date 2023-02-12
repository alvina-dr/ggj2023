using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : InteractableObject
{

    public int reachRepeater;
    public List<Rail> repeatedRails;


    public override void Start()
    {
        base.Start();
        DetectRails();
    }


    public void DetectRails()
    {
        repeatedRails.Clear();
        foreach (Rail _rail in GPCtrl.Instance.railList)
        {
            float _distance = Mathf.Sqrt(Mathf.Pow((_rail.gridPosition.x - gridPosition.x), 2)) + Mathf.Sqrt(Mathf.Pow((_rail.gridPosition.y - gridPosition.y), 2));
            if (_distance <= reachRepeater)
            {
                repeatedRails.Add(_rail);
                _rail.ActivateRail();
            }
        }
    }
}
