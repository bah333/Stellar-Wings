using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceEnvironment : MonoBehaviour
{
    public GameObject asteroidManagerPrefab; // self explanitory
    public GameObject player; // used for keeping track of location

    public float worldAsteroidDensity; // will use as a backup envronment method

    // method 2
    List<Vector3> Blocks; // a list of Vector 3s that corospond with abstract block coordinates that are filled with asteroids
    public int blockSize; // increases this to lower asteroid density



    // The following variables are determined when Instantiating asteroid groups
    // and will be different for each group
    float maxAsteroidSize;

    void Awake()
    {
        
        Blocks = new List<Vector3>();

        Vector3 currentBlock = DetermineBlockCoordinate();
        Debug.Log(currentBlock);
        Blocks.Add(currentBlock);
        FillBlock(currentBlock);





        // method 1  incomplete
        /*
        // on awake produces 4 asteroid managers in the vicinity of the player
        for (int i = 0; i < 4; i++)
        {
            float x = Random.Range(player.transform.position.x + 300f, player.transform.position.x + 1000f);
            if (Random.Range(1, 3) == 1)
                x *= -1;

            float y = Random.Range(player.transform.position.y + 300f, player.transform.position.y + 1000f);
            if (Random.Range(1, 3) == 1)
                y *= -1;

            float z = Random.Range(player.transform.position.z + 300f, player.transform.position.z + 1000f);
            if (Random.Range(1, 3) == 1)
                z *= -1;
          
            ProduceAsteroids(new Vector3(x, y, z));
            
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //ContinuousEnvironmentMethod1(); // method 1 is less accurate but cheaper on computations
        ContinuousEnvironmentMethod2(); // method 2 is fully accurate but more expensive on run time
    }

    // fills a block with asteroid managers
    void FillBlock(Vector3 block)
    {
        Debug.Log("filling block" + block);
        Vector3 centerOfBlock = block * blockSize;
        
        int density = Random.Range(3, 6);
        for (int i = 0; i < density; i++)
        {
            float x = centerOfBlock.x + Random.Range(-blockSize/2f, blockSize/2f);
            float y = centerOfBlock.y + Random.Range(-blockSize/2f, blockSize/2f);
            float z = centerOfBlock.z + Random.Range(-blockSize/2f, blockSize/2f);

            ProduceAsteroids(new Vector3(x, y, z));
        }
    }

    // determines the block coordinate
    Vector3 DetermineBlockCoordinate()
    {
        float num = player.transform.position.x;
        int xBlockCoordinate = 0;
        while(num > blockSize/2f)
        {
            xBlockCoordinate++;
            num -= blockSize;
        }
        while(num < -blockSize/2f)
        {
            xBlockCoordinate--;
            num += blockSize;
        }

        num = player.transform.position.y;
        int yBlockCoordinate = 0;
        while(num > blockSize/2f)
        {
            yBlockCoordinate++;
            num -= blockSize;
        }
        while(num < -blockSize/2f)
        {
            yBlockCoordinate--;
            num += blockSize;
        }

        num = player.transform.position.z;
        int zBlockCoordinate = 0;
        while(num > blockSize/2f)
        {
            zBlockCoordinate++;
            num -= blockSize;
        }
        while(num < -blockSize/2f)
        {
            zBlockCoordinate--;
            num += blockSize;
        }

        return new Vector3(xBlockCoordinate, yBlockCoordinate, zBlockCoordinate);
    }


    // checks the players location and ensures the players block and all adjacent blocks are filled with asteroids
    void ContinuousEnvironmentMethod2()
    {
        Vector3 currentBlock = DetermineBlockCoordinate();

        int x = 0; int y = 0; int z = 0;
        for(x = (int) currentBlock.x - 1; x < currentBlock.x + 2; x++)
            for(y = (int) currentBlock.y - 1; y < currentBlock.y + 2; y++)
                for(z = (int) currentBlock.z - 1; z < currentBlock.z + 2; z++)
                {
                    bool filled = false;
                    foreach( Vector3 block in Blocks)
                        if(block.Equals(new Vector3(x, y, z)))
                            filled = true;
                    if( ! filled)
                    {
                        Blocks.Add(new Vector3(x, y, z));
                        FillBlock(new Vector3(x, y, z));
                    }
                }
    }


    // incomplete    will complete and implement if method 2 is too costly on processing time
    /*
    void ContinuousEnvironmentMethod1()
    {
        Collider[] asteroidManagerColliders = Physics.OverlapSphere(player.transform.position, worldAsteroidDensity, 1 << 9, QueryTriggerInteraction.Collide);
  
        int asteroidsInRadius = 0;
        foreach (Collider col in asteroidManagerColliders)
        {
            asteroidsInRadius += col.gameObject.GetComponent<AsteroidManager>().GetAsteroidAmount();
        }
        Debug.Log(asteroidsInRadius);
        if (asteroidsInRadius < 200) // produce some asteroids somewhere within a cone centered along the players forward
        {
            Vector3 position = player.transform.position + player.transform.forward * Random.Range(worldAsteroidDensity/3f, worldAsteroidDensity);//player.transform.position + player.transform.forward * Random.Range(1500f, 3000f);
            /*
            float x = Random.Range(0f, player.transform.position.x + 500);
            if (Random.Range(1, 3) == 1)
                x *= -1;

            float y = Random.Range(0f, player.transform.position.y + 500);
            if (Random.Range(1, 3) == 1)
                y *= -1;

            float z = Random.Range(0f, player.transform.position.z + 500);
            if (Random.Range(1, 3) == 1)
                z *= -1;
           

   *  /
        
           //Debug.Log("Making more because we only have " + asteroidsInRadius);
           Debug.Log("MAde moew");
           ProduceAsteroids(position);
        }
    }
*/

    // produces an asteroid manager with random values at th locatipn given
    void ProduceAsteroids(Vector3 position)
    {
        GameObject newAsteroidManager;
        newAsteroidManager = Instantiate(asteroidManagerPrefab, position, new Quaternion(0, 0, 0, 1));

        // Set all the public values of the new Asteroid Manager

        // make not timed
        newAsteroidManager.GetComponent<AsteroidManager>().timedAsteroids = false;
        newAsteroidManager.GetComponent<AsteroidManager>().timedAsteroidLife = 10f;

        // determine asteroid radius Range
        // asteroid feilds with large asteroids are less likly so we do extra randomizing to increase chance of low sixes
        maxAsteroidSize = Random.Range(2f, 5.001f);
        maxAsteroidSize = Random.Range(1.5f, maxAsteroidSize);
        float minAsteroidSize = Random.Range(0.5f, maxAsteroidSize);
        if (minAsteroidSize > 1.4f)
            minAsteroidSize = Random.Range(0.5f, maxAsteroidSize);
        newAsteroidManager.GetComponent<AsteroidManager>().maxAsteroidRadius = maxAsteroidSize;
        newAsteroidManager.GetComponent<AsteroidManager>().minAsteroidRadius = minAsteroidSize;

        int materialInclusion = Random.Range(0, 11); // weighted so high health material is less likely
        if ((materialInclusion == 1 || materialInclusion == 2) || materialInclusion == 3) // only low
        {
            newAsteroidManager.GetComponent<AsteroidManager>().includeLowHealthMaterial = true;
            newAsteroidManager.GetComponent<AsteroidManager>().includeMediumHealthMaterial = false;
            newAsteroidManager.GetComponent<AsteroidManager>().includeHighHealthMaterial = false;
        }
        else if (materialInclusion == 4 || materialInclusion == 5) // low and medium
        {
            newAsteroidManager.GetComponent<AsteroidManager>().includeLowHealthMaterial = true;
            newAsteroidManager.GetComponent<AsteroidManager>().includeMediumHealthMaterial = true;
            newAsteroidManager.GetComponent<AsteroidManager>().includeHighHealthMaterial = false;
        }
        else if (materialInclusion == 6) // all types
        {
            newAsteroidManager.GetComponent<AsteroidManager>().includeLowHealthMaterial = true;
            newAsteroidManager.GetComponent<AsteroidManager>().includeMediumHealthMaterial = true;
            newAsteroidManager.GetComponent<AsteroidManager>().includeHighHealthMaterial = true;
        }
        else if (materialInclusion == 7 || materialInclusion == 8) // only medium
        {
            newAsteroidManager.GetComponent<AsteroidManager>().includeLowHealthMaterial = false;
            newAsteroidManager.GetComponent<AsteroidManager>().includeMediumHealthMaterial = true;
            newAsteroidManager.GetComponent<AsteroidManager>().includeHighHealthMaterial = false;
        }
        else if (materialInclusion == 9) // medium and high
        {   
            newAsteroidManager.GetComponent<AsteroidManager>().includeLowHealthMaterial = false;
            newAsteroidManager.GetComponent<AsteroidManager>().includeMediumHealthMaterial = true;
            newAsteroidManager.GetComponent<AsteroidManager>().includeHighHealthMaterial = true;
        }

        newAsteroidManager.GetComponent<AsteroidManager>().minSpawnVelocity = 0f;
        newAsteroidManager.GetComponent<AsteroidManager>().maxSpawnVelocity = Random.Range(1f, 5.1f);

        newAsteroidManager.GetComponent<AsteroidManager>().fieldMinVelocity = 0.1f;
        newAsteroidManager.GetComponent<AsteroidManager>().fieldMaxVelocity = 0.8f;

        int x = Random.Range(1, 4);
        if (x < 3) // make an asteroid field
        {
            newAsteroidManager.GetComponent<AsteroidManager>().isBelt = false;
            newAsteroidManager.GetComponent<AsteroidManager>().asteroidFieldRadius = 80f*(maxAsteroidSize);

            newAsteroidManager.GetComponent<AsteroidManager>().minAsteroidFieldCount = 30;
            newAsteroidManager.GetComponent<AsteroidManager>().maxAsteroidFieldCount = 80;
            if (x == 1)
            {
                newAsteroidManager.GetComponent<AsteroidManager>().fieldMoves = true;
            }
            else
                newAsteroidManager.GetComponent<AsteroidManager>().fieldMoves = false;
        }
        else // make an asteroid belt
        {
            newAsteroidManager.GetComponent<AsteroidManager>().isBelt = true;
            newAsteroidManager.GetComponent<AsteroidManager>().fieldMoves = true;
            newAsteroidManager.GetComponent<AsteroidManager>().fieldMoveRadius = Random.Range(350f + 15f * maxAsteroidSize, 500f + 20f * maxAsteroidSize);
            newAsteroidManager.GetComponent<AsteroidManager>().minAsteroidFieldCount = 45;
            newAsteroidManager.GetComponent<AsteroidManager>().maxAsteroidFieldCount = 200;
            newAsteroidManager.GetComponent<AsteroidManager>().beltWidth = 20f;
        }
        
    }
}
