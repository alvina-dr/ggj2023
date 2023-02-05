using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rail : InteractableObject
{

    public bool isActivated = false;
    public Material activeMaterial;
    public Material inactiveMaterial;

    public MeshRenderer runeA;
    public MeshRenderer runeB;


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
