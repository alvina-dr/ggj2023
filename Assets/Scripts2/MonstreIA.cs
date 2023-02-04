using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonstreIA : MonoBehaviour
{

    public GameObject target;


    //Agent de Navigation
    NavMeshAgent navMeshAgent;


    //Animations
    Animator animator;
    const string STAND_STATE = "Stand";
    const string WALK_STATE = "Walk";
    const string ATTACK_STATE = "Attack";

    //Action actuelle
    public string currentAction;

    bool isLoaded = true;
    public float reloadTime = .5f;


    // Start is called before the first frame update
    void Awake()
    {
        currentAction = STAND_STATE;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Amplificateur");
        if (target != null)
        {
            //Est-ce que l'IA se déplace vers le joueur ?
            if (MovingToTarget())
            {
                //En train de marcher
                return;
            }
            //Sinon c'est qu'elle est à distance d'attaque
            else
            {
                if (isLoaded)
                {
                    Attack();
                }
            }
        }
    }

    bool MovingToTarget()
    {
        //Assigne la destination : le joueur
        navMeshAgent.SetDestination(target.transform.position);

        //navMeshAgent pas prêt ?
        if (navMeshAgent.remainingDistance == 0)
            return false;


        // navMeshAgent.remainingDistance = distance restante pour atteindre la cible (Player)
        // navMeshAgent.stoppingDistance = à quel distance de la cible l'IA doit s'arrête 
        // (exemple 2 m pour le corps à sorps) 
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {

            if (currentAction != WALK_STATE)
                Walk();

        }
        else
        {
            //Si arrivé à bonne distance, regarde vers le joueur
            RotateToTarget(target.transform);
            return false;
        }

        return true;
    }


    //Walk = Marcher
    void Walk()
    {
        //Réinitialise les paramètres de l'animator
        ResetAnimation();
        //L'action est maintenant "Walk"
        currentAction = WALK_STATE;
        //Le paramètre "Walk" de l'animator = true
        animator.SetBool(WALK_STATE, true);
    }

    //Attack = Attaquer
    void Attack()
    {
        Debug.Log("ATTACK");
        //Réinitialise les paramètres de l'animator
        ResetAnimation();
        //L'action est maintenant "Attack"
        currentAction = ATTACK_STATE;
        //Le paramètre "Attack" de l'animator = true
        animator.SetBool(ATTACK_STATE, true);
        Shoot();
    }

    private void ResetAnimation()
    {
        animator.SetBool(WALK_STATE, false);
        animator.SetBool(ATTACK_STATE, false);
    }


    //Permet de tout le temps regarder en direction de la cible
    private void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }
    public void Shoot()
    {
        if (target.GetComponent<ReceiveDamage>() != null) { 
            target.GetComponent<ReceiveDamage>().hitPoint = target.GetComponent<ReceiveDamage>().hitPoint - 1;
            isLoaded = false;
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload()
    {
        Debug.Log("need to reload");
        yield return new WaitForSeconds(reloadTime);
        isLoaded = true;
        Debug.Log("reload ! ");
    }

    //public void SelectTarget()
    //{
    //     FindObjectsOfType<ReceiveDamage>()
    //        target =
    //}
}