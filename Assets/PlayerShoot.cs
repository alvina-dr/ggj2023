using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerShoot : MonoBehaviour
{

    bool isLoaded = true;
    public float reloadTime = .5f;
    List<MonstreIA> _targets = new List<MonstreIA>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MonstreIA>() != null)
        {
            _targets.Add(other.GetComponent<MonstreIA>());
        }
    }

    public void Update()
    {
        if (_targets.Count > 0 && _targets[0] == null) { _targets.Remove(_targets[0]); }
        if (_targets != null && _targets.Count > 0 && isLoaded)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (_targets[0].GetComponent<HealthManager>() != null)
        {
            _targets[0].GetComponent<HealthManager>().GetDamage(1);
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
