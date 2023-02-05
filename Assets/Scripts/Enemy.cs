using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed = 5;
    public int degat = 2;
    public int health = 5;

    public GameObject mob;  
    
    public GameObject player;
    //PlayerManager playerManager;

    //public ParticleSystem explosion;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerManager = player.GetComponent<PlayerManager>();

        if (health <= 0)
        {
            DestroyMob();
            //playerManager.points += 100;
        }

        /*if (this.transform.position.z < player.transform.position.z - 240)
        {
            DestroyMob();
        }*/
    }

    void DestroyMob()
    {
        //Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(mob);
    }


    
    void OnTriggerEnter(Collider attack)
    {
        if (attack.gameObject.tag == "laser")
        {
            health -= 1;
            Destroy(attack.gameObject);
        }
        if (attack.gameObject.tag == "gros laser")
        {
            health -= 5;
            Destroy(attack.gameObject);
        }

        //print("Another object has entered the trigger");
        
    }
}
