using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : InteractableObject
{

    public int reachRepeater;
    public List<Rail> repeatedRails;

    void Start()
    {
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
        List<InteractableObject> _rails = GPCtrl.Instance.objectList.FindAll(x => x.objectType == InteractableObject.ObjectType.Rail);
        foreach (InteractableObject _rail in _rails)
        {
            float _distance = Mathf.Sqrt(Mathf.Pow((_rail.gridPosition.x - gridPosition.x), 2)) + Mathf.Sqrt(Mathf.Pow((_rail.gridPosition.y - gridPosition.y), 2));
            if (_distance <= reachRepeater)
            {
                repeatedRails.Add(_rail.GetComponent<Rail>());
                _rail.GetComponent<Rail>().ActivateRail();
            }
        }
    }
}
