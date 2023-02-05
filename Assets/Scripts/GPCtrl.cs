using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GPCtrl : MonoBehaviour
{

    private static GPCtrl _instance;

    public static GPCtrl Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GPCtrl>();
            }

            return _instance;
        }
    }

    public Tilemap railMap;
    public List<TileBase> tileBaseTools = new List<TileBase>();
    public List<InteractableObject> objectList = new List<InteractableObject>();


    void Start()
    {
        UpdateObjectList();
        UpdateAllRailState();
    }

    public void UpdateObjectList()
    {
        objectList.Clear();
        InteractableObject[] _objects = FindObjectsOfType<InteractableObject>();
        for (int i = 0; i < _objects.Length; i++)
        {
            objectList.Add(_objects[i]);
        }
        if (objectList.Find(x => x.objectType == InteractableObject.ObjectType.Repeater) == null) { 
            Debug.Log("GAME OVER");
            LooseGame();
        }
    }

    public void UpdateAllRailState()
    {
        List<InteractableObject> _rails = objectList.FindAll(x => x.objectType == InteractableObject.ObjectType.Rail);
        foreach(InteractableObject _rail in _rails)
        {
            _rail.GetComponent<Rail>().DeactivateRail();
        }
        DetectAllRails();
    }

    public void DetectAllRails()
    {
        List<InteractableObject> _repeaters = objectList.FindAll(x => x.objectType == InteractableObject.ObjectType.Repeater);
        foreach (InteractableObject _repeater in _repeaters)
        {
            _repeater.GetComponent<Repeater>().DetectRails();
        }
    }

    public void WinGame()
    {
        SceneManager.LoadScene(0);
    }

    public void LooseGame()
    {
        SceneManager.LoadScene(0);
    }
}
