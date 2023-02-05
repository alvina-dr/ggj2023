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
        if (target != null) { 
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 12);
            transform.LookAt(target.position);
        } else
        {
            Destroy(gameObject);
        }
    }

}
