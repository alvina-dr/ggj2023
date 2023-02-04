using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ReceiveDamage : MonoBehaviour
{

    //Maximum de points de vie
    public int maxHitPoint = 5;

    //Points de vie actuels
    public int hitPoint = 0;

    //Apr�s avoir re�u un d�g�t :
    //La cr�ature est invuln�rable quelques instants
    public bool isInvulnerable;

    //Temps d'invuln�rabilit�
    public float invulnerabiltyTime;

    //Temps depuis le dernier d�g�t
    private float timeSinceLastHit = 0.0f;

    Collider attack;



    private void Start()
    {
        //Au d�but : Points de vie actuels = Maximum de points de vie
        hitPoint = maxHitPoint;

        isInvulnerable = false;
    }

    private void Update()
    {
        //Si invuln�rable
        if (isInvulnerable)
        {
            //Compte le temps depuis le dernier d�g�t
            //timeSinceLastHit = temps depuis le dernier d�g�t
            //Time.deltaTime = temps �coul� depuis la derni�re frame
            timeSinceLastHit += Time.deltaTime;

            if (timeSinceLastHit > invulnerabiltyTime)
            {
                //Le temps est �coul�, il n'est plus invuln�rable
                timeSinceLastHit = 0.0f;
                isInvulnerable = false;

            }
        }
    }

    //Permet de recevoir des dommages
    public void GetDamage(int damage)
    {
        if (isInvulnerable)
            return;

        isInvulnerable = true;

        //Applique les dommages aux points de vies actuels
        hitPoint -= damage;

        //S'il reste des points de vie
        if (hitPoint > 0)
        {
            //SendMessage appellera toutes les m�thodes "TakeDamage" de ce GameObject
            //Exemple : "TakeDamage" est dans MonsterController
            gameObject.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
        }
        //Sinon
        else
        {
            //SendMessage appellera toutes les m�thodes "Defeated" de ce GameObject
            //Exemple : "Defeated" est dans MonsterController
            gameObject.SendMessage("Defeated", SendMessageOptions.DontRequireReceiver);
        }
        if (hitPoint < 1)
        {
            Destroy(gameObject);
        }
    }

    //void OnTriggerEnter(Collider attack)
    //{
        
    //    //if (attack.GetComponent<MonstreIA>() != null)
    //    if (hitPoint > 0)
    //    {
    //        hitPoint -= 1;
    //    }else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

}