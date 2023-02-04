using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : MonoBehaviour
{

    public int reachRepeater;
    public List<Rail> repeatedRails;

    void Start()
    {
        repeatedRails.Clear();
        repeatedRails = GPCtrl.Instance.rails.FindAll(x => Mathf.Sqrt((x.transform.position.x)* (x.transform.position.x)) + Mathf.Sqrt((x.transform.position.y) * (x.transform.position.y)) <= reachRepeater);
    }

    void Update()
    {
        
    }
}
