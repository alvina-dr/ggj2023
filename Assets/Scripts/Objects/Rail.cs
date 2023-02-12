using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rail : MonoBehaviour
{

    public bool isActivated = false;
    public Material activeMaterial;
    public Material inactiveMaterial;

    public MeshRenderer runeA;
    public MeshRenderer runeB;


    public Vector3Int gridPosition;


    void Start()
    {
        gridPosition = GPCtrl.Instance.interactionMap.WorldToCell(transform.position);
    }

    public bool CheckIfRailActivated()
    {
        return isActivated;
    }

    public void ActivateRail()
    {
        runeA.material = activeMaterial;
        runeB.material = activeMaterial;
        isActivated = true;
    }

    public void DeactivateRail()
    {
        runeA.material = inactiveMaterial;
        runeB.material = inactiveMaterial;
        isActivated = false;
    }
}
