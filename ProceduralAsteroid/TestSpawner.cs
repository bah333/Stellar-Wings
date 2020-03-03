using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{

    public GameObject asteroid;
    public float initialDelay = 0.0f;
    public float delay = 2.5f;
    public float asteroidLifeTime = 4.0f;
    public Vector3 worldSpaceVelocity;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("instantiateAsteroid", initialDelay, delay);
    }

    void instantiateAsteroid()
    {
        //int randIndex = Random.Range(0, projectiles.Length);
        asteroid.GetComponent<ProceduralAsteroid>().materialType = Random.Range(1, 4);
        asteroid.GetComponent<ProceduralAsteroid>().sphereRadius = Random.Range(0.5f, 3.1f);
        asteroid.GetComponent<ProceduralAsteroid>().initialForwardVelocity = worldSpaceVelocity;
        
        GameObject newInstance = Instantiate(asteroid, this.gameObject.transform);
        //Destroy(newInstance, asteroidLifeTime);
        //Rigidbody rb = newInstance.GetComponent<Rigidbody>();
        //rb.velocity = new Vector3(0, 0, -2);
    }
}
