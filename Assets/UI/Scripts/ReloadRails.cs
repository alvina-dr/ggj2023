using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class ReloadRails : MonoBehaviour
    {

        [SerializeField] private Image fillBar;
    
        [SerializeField] private TextMeshProUGUI railCount;
        private float _elapsedTime;
        private float _timeToReload = 5f;
        private int _numberOfRails;
        private PlayerController _playerController;

        private void Awake()
        {
            fillBar.fillAmount = 0;
            _numberOfRails = 0;
        
        
        }

        // Start is called before the first frame update
        void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            railCount.text = _playerController.railAmmo.ToString() + "/" + _playerController.railAmmoMax.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            railCount.text = _playerController.railAmmo.ToString() + "/" + _playerController.railAmmoMax.ToString();
            if (_elapsedTime < _playerController.railReloadTime)
            {
                _elapsedTime += Time.deltaTime;
                fillBar.fillAmount = _elapsedTime / _playerController.railReloadTime;

                if (fillBar.fillAmount == 1f)
                {
                    fillBar.fillAmount = 0;
                    _elapsedTime = 0f;
                }
            }

            if (_playerController.railAmmo == _playerController.railAmmoMax)
            {
                fillBar.fillAmount = 0;
            }
        }

        private void FillProgress(float value)
        {

       
        
        
        }

        public void UpdateRailBar()
        {
            railCount.text = _playerController.railAmmo.ToString() + "/" + _playerController.railAmmoMax.ToString();
 
        }
    }
}
