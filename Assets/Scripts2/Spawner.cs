using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private float timer = 0f;
    [SerializeField]
    private float timeSpawn = 10f;
    [SerializeField]
    private GameObject mob;
    [SerializeField]
    private int minMob = 8;
    [SerializeField]
    private int maxMob = 12;
    [SerializeField]
    public int randomSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(timeSpawn, mob));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeSpawn)
        {
            randomSpawn = Random.Range(minMob, maxMob);
            for(int i = 0; i < randomSpawn; i++)
            {
                GameObject _mob = Instantiate(mob);
                _mob.transform.position = transform.position;
            }
            timer = 0f;
        }
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, newEnemy));
    }
}
