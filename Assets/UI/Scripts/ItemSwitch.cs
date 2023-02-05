using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSwitch : MonoBehaviour
{
    [SerializeField] private List<Sprite> itemSprites;
    
    [SerializeField] private Image spritesContainer;

    private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        spritesContainer.sprite = itemSprites[_playerController.currentTool];
    }
}
