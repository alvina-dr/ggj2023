using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ReloadRails : MonoBehaviour
{
    [SerializeField] private Image fillBar;
    [SerializeField] private TextMeshProUGUI railCount;
    private float _elapsedTime;
    private float _timeToReload = 5f;
    private int _numberOfRails;

    private void Awake()
    {
        fillBar.fillAmount = 0;
        _numberOfRails = 0;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_elapsedTime < _timeToReload)
        {
            _elapsedTime += Time.deltaTime;
            fillBar.fillAmount = _elapsedTime / _timeToReload;

            if (fillBar.fillAmount == 1)
            {
                _numberOfRails++;
                railCount.text = _numberOfRails.ToString();
                fillBar.fillAmount = 0;
                _elapsedTime = 0f;
            }
        }
    }

    private void FillProgress(float value)
    {

       
        
        
    }
}
