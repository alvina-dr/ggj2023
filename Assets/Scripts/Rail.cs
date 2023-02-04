using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rail : MonoBehaviour
{

    public bool isActivated = false;
    public Vector3Int gridPosition;
    void Start()
    {
        gridPosition = GPCtrl.Instance.railMap.WorldToCell(transform.position);
    }

    void Update()
    {
        
    }

    public bool CheckIfRailActivated()
    {
        return isActivated;
    }

    public void ActivateRail()
    {
        transform.DOScale(1.1f, .2f);
        transform.DOMoveY(-.2f, .2f);
        isActivated = true;
    }
}
