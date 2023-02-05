using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HealthManager : MonoBehaviour
{

    public int maxHealth = 5;

    public int currentHealth = 0;

    GameObject projectile;

    AudioSource enemy_death;
    AudioSource repeater_stop;
    public GameObject son;
    SelectAudioSource audioSource;


    void Start()
    {
        currentHealth = maxHealth;

        enemy_death = son.GetComponent<SelectAudioSource>().enemy_death;
        repeater_stop = son.GetComponent<SelectAudioSource>().repeater_stop;
    }

    private void Update()
    {
    }

    public void GetDamage(int damage)
    {

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            GPCtrl.Instance.UpdateObjectList();
            if (GetComponent<Repeater>()!= null)
            {
                repeater_stop.Play(0);
            }
            else
            {
                enemy_death.Play(0);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponentInParent<Projectile>() != null && collision.GetComponentInParent<Projectile>().target == transform) 
        {
            GetDamage(collision.GetComponentInParent<Projectile>().damage);
            Destroy(collision.transform.parent.gameObject);
        }

    }
}