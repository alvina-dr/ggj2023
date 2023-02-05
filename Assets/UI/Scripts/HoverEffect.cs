using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

namespace UI.Scripts
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private TextMeshProUGUI changedText;

        public GameObject SelectSprite;
        
        public Color BaseColor;
        public Color HoverColor;
    
        // Start is called before the first frame update
        void Start()
        {
            changedText.color = BaseColor;
            SelectSprite.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            changedText.color = HoverColor;
            transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f),0.2f);
            SelectSprite.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            changedText.color = BaseColor;
            transform.DOScale(new Vector3(1f, 1f, 1f),0.2f);
            SelectSprite.SetActive(false);
        }
    }
}
