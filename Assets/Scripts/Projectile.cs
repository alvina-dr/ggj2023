using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed = 20;
    public int lifeTime = 300;
    private int timeForLife = 0;

    public int damage;
    public Transform target;


    void Update()
    {
        //timeForLife += 1;
        //if (timeForLife < lifeTime)
        //{
        //    transform.Translate(Vector3.forward * Time.deltaTime * speed);
        //}
        //if (timeForLife >= lifeTime)
        //{
        //    Destroy(gameObject);
        //}
        if (target != null) { 
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 12);
            transform.LookAt(target.position);

        } else
        {
            Debug.Log("target null"); 
            Destroy(gameObject);

        }

        //if ( transform.position == target.position)
        //{
        //    Debug.Log("has reached point");
        //    Destroy(gameObject);
        //}
    }

}
