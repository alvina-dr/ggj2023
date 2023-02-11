using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : InteractableObject
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

    AudioSource wave_of_enemies;
    public GameObject son;
    SelectAudioSource audioSource;

    void Start()
    {
        StartCoroutine(spawnEnemy(timeSpawn, mob));

        wave_of_enemies = son.GetComponent<SelectAudioSource>().wave_of_enemies;
    }

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
            wave_of_enemies.Play(0);
        }
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, newEnemy));
    }
}
