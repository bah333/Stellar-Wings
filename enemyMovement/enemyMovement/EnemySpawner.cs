using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    GameObject player;
    [SerializeField] private GameObject enemy;
    GameObject[] enemiesList;

 
    public float playerRadius;
    public int maxEnemyCount;
    Vector3 randPosition;
    public float distanceApart;

    void Start()
    {

        distanceApart = 5.0f;
        playerRadius = 40.0f;
        maxEnemyCount = 5;
        randPosition = new Vector3(0, 0, 0);

        float secondsToFirstCall = 2.0f;
        float secondsToWait = 0.6f;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("player");
        }

        enemiesList = new GameObject[maxEnemyCount];
        for (int i = 0; i < maxEnemyCount; ++i)
        {
            enemiesList[i] = null;
        }


    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }


    void SpawnEnemy()
    {
        for (int i = 0; i < maxEnemyCount; ++i)
        {
            if (enemiesList[i] == null)
            {
                enemiesList[i] = Instantiate(enemy) as GameObject;

                // randPosition = a random location within a sphere of radius 1 * playerRadius (expands the radius of sphere) at (+) the player's current position
                randPosition =  Random.insideUnitSphere * playerRadius + player.transform.position;

                enemiesList[i].transform.position = randPosition;
            }

        }
    }
}

