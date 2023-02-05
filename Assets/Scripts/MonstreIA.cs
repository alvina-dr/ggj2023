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
    public Projectile projectilePrefab;
    public int damage;


    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target != null)
        {
            if (MovingToTarget())
            {
                return;
            }
            else
            {
                if (isLoaded)
                {
                    Attack();
                }
            }
        } 
        else
        {
            SelectTarget();
        }
    }

    bool MovingToTarget()
    {
        navMeshAgent.SetDestination(target.transform.position);

        if (navMeshAgent.remainingDistance == 0)
            return false;

        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {



        }
        else
        {
            RotateToTarget(target.transform);
            return false;
        }

        return true;
    }




    void Attack()
    {
        Shoot();
    }



    private void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }
    public void Shoot()
    {
        if (target.GetComponent<Repeater>() != null)
        {
            Projectile _proj = Instantiate(projectilePrefab);
            _proj.transform.position = new Vector3(transform.position.x, .5f, transform.position.z);
            _proj.transform.LookAt(target.transform.position);
            _proj.target = target.transform;
            _proj.damage = damage;
            Debug.Log(damage);
            isLoaded = false;
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        isLoaded = true;
    }

    public void SelectTarget()
    {
        Repeater[] _repeatersArr = FindObjectsOfType<Repeater>();
        List<Repeater> _repeaters = new List<Repeater>();
        foreach (Repeater _repeater in _repeatersArr)
        {
            _repeaters.Add(_repeater);
        }
        float minDistance = 1000;
        foreach(Repeater _repeater in _repeaters)
        {
            float _currentDistance = Vector3.Distance(transform.position, _repeater.transform.position);
            if (_currentDistance < minDistance) { minDistance = _currentDistance; }
        }
        if (_repeaters.Count == 0)
        {
            Debug.Log("no repeaters found");
        }
        target = _repeaters.Find(x => Vector3.Distance(transform.position, x.transform.position) == minDistance).gameObject;
    }
}