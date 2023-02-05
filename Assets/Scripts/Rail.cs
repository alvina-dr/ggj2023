using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rail : MonoBehaviour
{

    public bool isActivated = false;
    public Vector3Int gridPosition;
    public Material activeMaterial;
    public Material inactiveMaterial;

    public MeshRenderer runeA;
    public MeshRenderer runeB;
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
        //transform.DOScale(1.1f, .2f);
        transform.DOMoveY(.05f, .2f);
        runeA.material = activeMaterial;
        runeB.material = activeMaterial;
        isActivated = true;
    }

    public void DeactivateRail()
    {
        //transform.DOScale(1f, .2f);
        transform.DOMoveY(0f, .2f);
        runeA.material = inactiveMaterial;
        runeB.material = inactiveMaterial;
        isActivated = false;
    }
}
