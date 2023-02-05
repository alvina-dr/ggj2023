using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class circle_tracks_fx : MonoBehaviour
{

    // ------------------------------------- NOT IMPORTANT -------------------------------------
    //public GameObject prefab = LoadAssetAtPath("Assets/GameJam23/Prefab/magic_ring Variant.prefab");
    //public GameObject prefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ------------------------------------- DOWN KEY -------------------------------------
        if (Input.GetKeyDown("down"))
        {
            
            //Instantiate(gameObject, new Vector3(-v, 1, 12), Quaternion.Euler(-90, 0, 0));
            //gameObject.transform.DOMoveX(transform.position.x - 2, 1);

            // ------------------------------------- SCALE CHANGE -------------------------------------
            gameObject.transform.DOScale(new Vector3(gameObject.transform.localScale.x + 3, gameObject.transform.localScale.y + 3, gameObject.transform.localScale.z), 1);

            // ------------------------------------- DEBUG -------------------------------------
            Debug.Log("DOWN key was pressed.");
            Debug.Log(gameObject.transform.position.x);
            Debug.Log(gameObject.transform.localScale);
        }

        // ------------------------------------- UP KEY -------------------------------------
        if (Input.GetKeyDown("up"))
        {

            gameObject.transform.DOScale(new Vector3(gameObject.transform.localScale.x + 3, gameObject.transform.localScale.y + 3, gameObject.transform.localScale.z + 3), 1);

            // ------------------------------------- DEBUG -------------------------------------
            Debug.Log("UP key was pressed.");

        }

    }
}
