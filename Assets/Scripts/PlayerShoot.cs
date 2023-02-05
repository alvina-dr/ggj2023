using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerShoot : MonoBehaviour
{

    bool isLoaded = true;
    public float reloadTime = .5f;
    public List<MonstreIA> _targets = new List<MonstreIA>();
    public Projectile projectilePrefab;
    public int damage;

    AudioSource robot_attack;
    public GameObject son;
    SelectAudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MonstreIA>() != null)
        {
            _targets.Add(other.GetComponent<MonstreIA>());
        }
    }

    private void Start()
    {
        robot_attack = son.GetComponent<SelectAudioSource>().robot_attack;
    }

    public void Update()
    {
        if (_targets.Count > 0 && _targets[0] == null) { _targets.Remove(_targets[0]); }
        if (_targets != null && _targets.Count > 0 && isLoaded)
        {
            Shoot();
            robot_attack.Play(0);
        }
        if (_targets != null && _targets.Count > 0 && _targets[0] != null)
        {
            transform.LookAt(_targets[0].transform.position);
        }
    }

    public void Shoot()
    {
        if (_targets[0].GetComponent<HealthManager>() != null)
        {
            Projectile _proj = Instantiate(projectilePrefab);
            _proj.transform.position = transform.position;
            _proj.transform.LookAt(_targets[0].transform.position);
            _proj.target = _targets[0].transform;
            _proj.damage = damage;
            isLoaded = false;
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        isLoaded = true;
    }

}
