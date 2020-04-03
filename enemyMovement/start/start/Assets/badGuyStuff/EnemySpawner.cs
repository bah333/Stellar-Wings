using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    GameObject player;
    [SerializeField] private GameObject enemy;
    public GameObject[] enemiesList;


    public float playerRadius;
    public int maxEnemyCount;
    Vector3 randPosition;
    public float distanceApart;
    float minOffset;
    float maxOffset;

    void Awake()
    {
        distanceApart = 5.0f;
        playerRadius = 300.0f;
        minOffset = -20.0f;
        maxOffset = 20.0f;
        maxEnemyCount = 5;
        randPosition = new Vector3(0, 0, 0);

        if (player == null)
            player = GameObject.FindGameObjectWithTag("player");

        enemiesList = new GameObject[maxEnemyCount];
    }

    // Update is called once per frame
    void Update()
    {
        spawnEnemy();
    }

    void spawnEnemy()
    {
        for (int i = 0; i < maxEnemyCount; ++i)
        {
            if (enemiesList[i] == null)
            {
                float temp1 = Random.Range(minOffset, maxOffset);
                float temp2 = Random.Range(minOffset, maxOffset);
                float temp3 = Random.Range(minOffset, maxOffset);
                Vector3 tempVector = new Vector3(temp1, temp2, temp3);

                enemiesList[i] = Instantiate(enemy) as GameObject;

                // randPosition = a random location within a sphere of radius 1 * playerRadius (expands the radius of sphere) at (+) the player's current position
                //randPosition = Random.insideUnitSphere * playerRadius + player.transform.position;

                randPosition = Random.onUnitSphere * playerRadius + player.transform.position;
                randPosition += tempVector;

                enemiesList[i].transform.position = randPosition;

            }
        }
    }

}

