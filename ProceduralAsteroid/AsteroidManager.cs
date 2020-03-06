/*****************************

    AsteroidManager.cs
    Brandon Alex Huzil
    manages a belt or field of asteroids by instantiating them and helping in their shattering
    also handles field and belt movement

*****************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AsteroidManager : MonoBehaviour
{
    public GameObject asteroid;

    public bool timedAsteroids = false; // whether or not asteroids shatter after a certian amount of time
    public float timedAsteroidLife = 10.0f; // how many seconds before asteroids break
    public float minAsteroidRadius = 0.5f; // the minimum radius of asteroids that spawn, not shatterd asteroid chunks tho
    public float maxAsteroidRadius = 3f; // the maximum radius of spawned asteroids
    public bool includeLowHealthMaterial = true; // whether or not low health asteroids will spawn
    public bool includeMediumHealthMaterial = false; // whether or not medium health asteroids will spawn
    public bool includeHighHealthMaterial = false; // whether or not high health asteroids will spawn
    int materialType = 0;
    public float minSpawnVelocity = 0f; // the minimum velocity that asteroids will spawn with in a random direction
    public float maxSpawnVelocity = 5f; // the minimum velocity that asteroids will spawn with in a random direction

    public int minAsteroidFieldCount = 1; // the minimum amount of asteroids that will spawn in a field, if just on asteroid is spawned at will spwan at the asteroid managers location and will not move as a field or belt
    public int maxAsteroidFieldCount = 20; // the maximum amount of asteroids that will spawn in a field
    public float asteroidFieldRadius = 300f; // the radius of the field it has more then 1 asteroid and is not a belt

    public bool fieldMoves = true; // whether or not a field moves in a circle or a belt rotates
    public float fieldMinVelocity = 0f; // the minimum speed of a moving field or rotating belt
    public float fieldMaxVelocity = 0.8f; // the maximum  speed of a moving field or rotating belt
                                        // the actual speed is a random number between these values
    float fieldMoveSpeed;
    public float fieldMoveRadius = 1000; // the radius of the cirlce a moving feild follows or of the center of a belts circle to the actual belt

    public bool isBelt = false; // whether is a field or a belt
    public float beltWidth = 15f; // the width of a belt
    Rigidbody rb;



    float r;
    int m;
    Vector3 v;
    Vector3 p;



void Awake()
{
    rb = GetComponent<Rigidbody>();
}

    // Start is called before the first frame update
    void Start()
    {
        // decide how many asteroids in the field
        int fieldSize = Random.Range(minAsteroidFieldCount, maxAsteroidFieldCount + 1);
        int directionX, directionY, directionZ;

        if (fieldSize == 1) // instantiate the single asteroid at the position of the asteroid manager
        {
            //for initial velocity because we have a max and min plus directions in both pos and neg
            //we need to get a random velocity between min and max and make in randomly in po or neg direction
            directionX = Random.Range(0, 2);
            if (directionX == 0)
                directionX = -1;
            directionY = Random.Range(0, 2);
            if (directionY == 0)
                directionY = -1;
            directionZ = Random.Range(0, 2);
            if (directionZ == 0)
                directionZ = -1;
            getMaterialType();
            InstantiateAsteroid(Random.Range(minAsteroidRadius, maxAsteroidRadius + 0.0001f), materialType, new Vector3(directionX * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f), directionY * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f), directionZ * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f)), transform.position);
        }
        else if (isBelt == false)
        {
            BuildBasicField(fieldSize);
        }
        else
        {
            BuildBelt(fieldSize);
        }
        
        fieldMoveSpeed = Random.Range(fieldMinVelocity, fieldMaxVelocity);
    }

    float angle = 0;
    // Update is called once per frame
    void Update()
    {
        if(!isBelt && fieldMoves) // move a field around in a big circle
        {
            angle += Time.deltaTime * (fieldMoveSpeed / (fieldMoveRadius/10f));
            float x = Mathf.Cos (angle) * fieldMoveRadius;
            float z = Mathf.Sin (angle) * fieldMoveRadius;
            float y = 0;
            transform.position = new Vector3 (x, y, z);
        }
        else if (isBelt && fieldMoves) // rotate the belt
        {
            transform.Rotate(0f, 0.01f * fieldMoveSpeed, 0f, Space.Self);
        }
    }

    // for use by the asteroid spawns as we cant handle construction our selves imagine r, m, v, and p as 
    // the paramaters we would pass to an asteroid constructor
    // spawned asteroids call this function to grab these paramaters
    public void GetConstructionVals(out float r, out int m, out Vector3 v, out Vector3 p)
    {
        r = this.r;
        m = this.m;
        v = this.v;
        p = this.p;
    }

    // instantiates a single asteroid and if timedAsteroids then also begin a coroutine to shatter asteroids after
    // the correct delay
    // it also sets the paramaters the asteroid will grab from GetConstructionVals
    public void InstantiateAsteroid(float radius, int materialType, Vector3 initialVelocity, Vector3 position)
    {
        r = radius;
        m = materialType;
        v = initialVelocity;
        p = position;

        GameObject newInstance = Instantiate(asteroid, transform);

        if (timedAsteroids)
        {
            StartCoroutine(ShatterAsteroid(newInstance));
        }
    }

    // the coroutine for times asteroids
    IEnumerator ShatterAsteroid(GameObject asteroid)
    {
        yield return new WaitForSeconds(timedAsteroidLife);

        if (asteroid != null)
        {
            asteroid.GetComponent<ProceduralAsteroid>().health = 0;
            asteroid.GetComponent<ProceduralAsteroid>().SubtractHealth(0);
        }
    }

    // picks a random material type of the availible material types
    // if no types are enabled asteroids will spawn as a pink default material
    void getMaterialType()
    {
        // the following if else block determines what material type to apply the the asteroid to be instantiated
        if (!includeLowHealthMaterial && (!includeMediumHealthMaterial && includeHighHealthMaterial))
            materialType = 3;
        else if (!includeLowHealthMaterial && (includeMediumHealthMaterial && !includeHighHealthMaterial))
            materialType = 2;
        else if (!includeLowHealthMaterial && (includeMediumHealthMaterial && includeHighHealthMaterial))
            materialType = Random.Range(2, 4);
        else if (includeLowHealthMaterial && (!includeMediumHealthMaterial && !includeHighHealthMaterial))
            materialType = 1;
        else if (includeLowHealthMaterial && (!includeMediumHealthMaterial && includeHighHealthMaterial))
        {
            materialType = Random.Range(1, 3);
            if (materialType == 2)
                materialType = 3;
        }
        else if (includeLowHealthMaterial && (includeMediumHealthMaterial && !includeHighHealthMaterial))
            materialType = Random.Range(1, 3);
        else if (includeLowHealthMaterial && (includeMediumHealthMaterial && includeHighHealthMaterial))
            materialType = Random.Range(1, 4);
    }

    // builds an asteroid field
    void BuildBasicField(int fieldSize)
    {
        int directionX, directionY, directionZ;
        for (int i = 0; i < fieldSize; i++) // runs for each asteroid
        {
            getMaterialType();
            
            directionX = Random.Range(0, 2);
            if (directionX == 0)
                directionX = -1;
            directionY = Random.Range(0, 2);
            if (directionY == 0)
                directionY = -1;
            directionZ = Random.Range(0, 2);
            if (directionZ == 0)
                directionZ = -1;
            float x = Random.Range(-1f, 1.0001f);
            float y = Random.Range(-1f, 1.0001f);
            float z = Random.Range(-1f, 1.0001f);
            // the 2nd new vector3 givin to Instantiate asteroid is a point in the shpere found by picking a random direction vector and projecting out a distance between 0 and radius
            // this generates an asteroid field though they apear more often near the center because those with less projection will have naturally less distance between each other then thos of greater projection however this is an accurate felling distribution
            InstantiateAsteroid(Random.Range(minAsteroidRadius, maxAsteroidRadius + 0.0001f), materialType, new Vector3(directionX * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f), directionY * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f), directionZ * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f)), new Vector3(x, y, z) * Random.Range(0f, asteroidFieldRadius + 0.001f));
        }
    }

    // builds an asteroid belt
    void BuildBelt(int fieldSize)
    {
        int directionX, directionY, directionZ;
        for (int i = 0; i < fieldSize; i++) // runs once for each asteroid
        {
            getMaterialType();
        
            directionX = Random.Range(0, 2);
            if (directionX == 0)
                directionX = -1;
            directionY = Random.Range(0, 2);
            if (directionY == 0)
                directionY = -1;
            directionZ = Random.Range(0, 2);
            if (directionZ == 0)
                directionZ = -1;
            float x = Random.Range(-1f, 1.0001f);
            float z = Random.Range(-1f, 1.0001f);

            // asteroid positions an made by projecting an asteroid along the x world space axis out to the correct distance then randomly rotating the belt along the y self space axis
            Vector3 asteroidPosition = new Vector3(1, 0, 0) * Random.Range(fieldMoveRadius - beltWidth/2f, fieldMoveRadius + beltWidth/2f + 0.001f);
            transform.Rotate(0f, Random.Range(0f, 360.0001f), 0f, Space.Self);
            asteroidPosition.y = Random.Range(transform.position.y - 6f, transform.position.y + 6f);

            InstantiateAsteroid(Random.Range(minAsteroidRadius, maxAsteroidRadius + 0.0001f), materialType, new Vector3(directionX * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f), directionY * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f), directionZ * Random.Range(minSpawnVelocity, maxSpawnVelocity + 0.0001f)), asteroidPosition);
        }
    }

}
