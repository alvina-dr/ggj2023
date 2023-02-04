using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public List<Rail> rails = new List<Rail>();
    public List<Repeater> repeaters = new List<Repeater>();


    void Start()
    {
        Rail[] _rails = FindObjectsOfType<Rail>();
        for (int i = 0; i < _rails.Length; i++)
        {
            rails.Add(_rails[i]);
        }
    }

    void Update()
    {
        
    }

    public void UpdateRailList()
    {
        rails.Clear();
        Rail[] _rails = FindObjectsOfType<Rail>();
        for (int i = 0; i < _rails.Length; i++)
        {
            rails.Add(_rails[i]);
        }
    }

    public void UpdateRepeaterList()
    {
        repeaters.Clear();
        Repeater[] _rails = FindObjectsOfType<Repeater>();
        for (int i = 0; i < _rails.Length; i++)
        {
            repeaters.Add(_rails[i]);
        }
        if (repeaters.Count == 0)
        {
            Debug.Log("GAME OVER");
        }
    }
}
