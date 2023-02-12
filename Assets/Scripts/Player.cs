using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int speed = 10;
    public int degat = 5;
    public int health = 50;

    public GameObject mob;

    public GameObject player;

    void OnTriggerEnter(Collider attack)
    {
        if (attack.gameObject.tag == "poing")
        {
            health -= 1;
            Destroy(attack.gameObject);
        }
        if (attack.gameObject.tag == "hache")
        {
            health -= 5;
            Destroy(attack.gameObject);
        }

        //print("Another object has entered the trigger");

    }
}
