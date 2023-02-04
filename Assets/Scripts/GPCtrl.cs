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
    public TileBase railTile;
    public List<Rail> rails = new List<Rail>();


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
}
