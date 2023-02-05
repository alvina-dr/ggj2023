using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmplificateurUI : MonoBehaviour
{
    [SerializeField]
    private Image fillBar;

    [SerializeField] private TextMeshProUGUI repeaterText;

    private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        repeaterText.text = _playerController.repeaterAmount.ToString();
        fillBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillBarUpdate()
    {
         float imageFillAmount = (float)_playerController.energyAmount / _playerController.energyMax;
         Debug.Log(imageFillAmount);
         if (imageFillAmount < fillBar.fillAmount)
         {
             fillBar.DOFillAmount(1, .2f)
                 .OnComplete( () =>
                 {
                     fillBar.fillAmount = 0;
                     fillBar.DOFillAmount(imageFillAmount, .2f);
                 });             
         }
         else
         {
             fillBar.DOFillAmount(imageFillAmount, .2f);
         }

    }

    public void CounterTextUpdate()
    {
        repeaterText.text = _playerController.repeaterAmount.ToString();
    }
    
    
}
