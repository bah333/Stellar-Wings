/*****************************

    ProceduralAsteroid.cs
    Brandon Alex Huzil
    builds a procedurally generated asteroid and handles its health
    on collisions

*****************************/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
 
 [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]

 // this is the code for an Asteroid
 // it is procedurally generated based off of an Icosphere morphing
 // it spawns smaller asteroids depending on its size when health <= 0
 // asteroids can collide with eachother
 // its starting health health calculation is based off of volume of a sphere,
 // using its sphereRadius and material type
 // this is the health formula
 // health = materialType * ((int) (4*Mathf.PI*sphereRadius*sphereRadius*sphereRadius/3) ) + 5;
public class ProceduralAsteroid : MonoBehaviour
{
    Mesh mesh;
    //MeshCollider meshCollider;
    Rigidbody rb;

    Vector3 eulerAngleVelocity;
    public float maxDirectionalRotation = 20.0f;

    Vector3[] vertices;

    List<int> triangles;

    //public GameObject cube; // for testing purposes
    float t; // used to calculate points of Icosahedron
    public int maxAsteroidNoise = 30; // although public I recomend 30
    public float sphereRadius = 1.0f; // the radius of the sphere before morhping
    public float radiusVariationRange = 0.5f; // although public I recomend 0.5f, this number is scaled by sphereRadius when actually used for morphing

    public Material lowHealthMaterial; // !MUST BE FILLED IN INSPECTOR! The material that is applied to low health asteroids
    public Material mediumHealthMaterial; // !MUST BE FILLED IN INSPECTOR! The material that is applied to medium health asteroids
    public Material highHealthMaterial; // !MUST BE FILLED IN INSPECTOR! The material that is applied to high health asteroids

    public int health; // must be altered only by SubtractHealth() or in inspector during run time for testing, if set to zero in inspector during run time it will not be destroyed until a health reducing collision happens

    public int materialType; // !MUST BE FILLED IN INSPECTOR AS 1, 2, OR 3, OR STATED BY WHATEVER INSTANTIATES ASTEROID AS 1, 2, OR 3! 1 is low, 2 is medium, 3 is high health material will be used

    public float drag = 0.5f; // although public i recomend 0.5f, alternativly can be 0 to allow moving asteroids to travel forever
    public Vector3 initialForwardVelocity; // !MUST BE DEFINED IN INSPECTOR OR WHATEVER INSTANTIATES ASTEROID!, this is in world space

    public GameObject asteroid; // must be filled with asteroid prefab




    // this is the list of vertices in proper
    // order to form the mesh for an icosahedron
    // grouped in triangles for easy reading
    int[] baseIcosehedronTriangleIndexOrder = 
    {0, 11, 5,    0, 5, 1,    0, 1, 7,    0, 7, 10,
    0, 10, 11,    1, 5, 9,    5, 11, 4,   11, 10, 2,
    10, 7, 6,     7, 1, 8,    3, 9, 4,    3, 4, 2, 
    3, 2, 6,      3, 6, 8,    3, 8, 9,    4, 9, 5,
    2, 4, 11,     6, 2, 10,   8, 6, 7,    9, 8, 1};

    // ********************************************************************************

    // add code to this function when collisions are registerd
    // call subtract health when you want the asteroid to take damage
    // please only let Alex (Brandon Huzil) edit any code outside of this function
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Ship")
        {
            SubtractHealth((int) (collision.relativeVelocity.magnitude / 2.0f));
        }
    }
    //********************************************************************************

    // Because unity handles class construction, not us we need to call this function to get info
    // from an Asteroid Manager if this asteroid was instantiated from a manager
    void PseudoConstructor()
    {
        // if this asteroid comes from an Asteroid Manager
        if (transform.parent != null)
        {
            float r;
            int m;
            Vector3 v;
            Vector3 p;

            transform.parent.GetComponent<AsteroidManager>().GetConstructionVals(out r, out m, out v, out p);

            sphereRadius = r;
            materialType = m;
            initialForwardVelocity = v;
            transform.position = p;
        }
    }

	void Awake()
    {
        PseudoConstructor();
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        rb.velocity = initialForwardVelocity;

        if (materialType ==  1)
            GetComponent<Renderer>().sharedMaterial = lowHealthMaterial;
        else if (materialType ==  2)
            GetComponent<Renderer>().sharedMaterial = mediumHealthMaterial;
        else if (materialType ==  3)
            GetComponent<Renderer>().sharedMaterial = highHealthMaterial;

        vertices = new Vector3[48];
        triangles = new List<int>();
    }

    void Start()
    {
        // calculate starting health
        health = materialType * ((int) (4*Mathf.PI*sphereRadius*sphereRadius*sphereRadius/3) ) + 5;
    

        // slight sphere dismorphing by altering spiker and denter
        // this adds natural roughnerss and unevenness
        float spiker = Random.Range(0.5f, 1.5f) / sphereRadius;
        float denter = Random.Range(1.5f, 2.5f) / sphereRadius;
        //float spiker = 1.0f / sphereRadius;
        //float denter = 2.0f / sphereRadius;
        t = (spiker + Mathf.Sqrt(5.0f)) / denter;
        DesignIcosahedrone();
        RefineIcosphere();
        MorphIcosphere();
        ProduceIcosphere();

        // set the rotation of the asteroid
        float x = Random.Range(-maxDirectionalRotation, maxDirectionalRotation + 0.1f);
        float y = Random.Range(-maxDirectionalRotation, maxDirectionalRotation);
        float z = Random.Range(-maxDirectionalRotation, maxDirectionalRotation + 0.1f);
        eulerAngleVelocity = new Vector3(x, y, z);
    }

    // used for setting starting rotation
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
    void Update() 
    {            
      
    }

    // create the vertices that lie on the base icosahedron
    void DesignIcosahedrone()
    {
        float radius = sphereRadius;
        // build vertices for icosahedron
        vertices[0] = new Vector3(-radius, t, 0);
        vertices[1] = new Vector3(radius, t, 0);
        vertices[2] = new Vector3(-radius, -t, 0);
        vertices[3] = new Vector3(radius, -t, 0);

        vertices[4] = new Vector3(0, -radius, t);
        vertices[5] = new Vector3(0, radius, t);
        vertices[6] = new Vector3(0, -radius, -t);
        vertices[7] = new Vector3(0, radius, -t);

        vertices[8] = new Vector3(t, 0, -radius);
        vertices[9] = new Vector3(t, 0, radius);
        vertices[10] = new Vector3(-t, 0, -radius);
        vertices[11] = new Vector3(-t, 0, radius);
    }

    // convert icosahedron into an icosphere
    void RefineIcosphere()
    {
        // now we take each triangle and break it into 4 smaller triangles
        // though we cant do this with all of the triangles or we will have 
        // double the nessicary vertices and wont be able to properly deform
        // the icosphere
        int verticesIndex = 12;
        int oldVertexPosition;
        int a, b, c;

        Vector3 potentialNewVertex1 = new Vector3();
        Vector3 potentialNewVertex2 = new Vector3();
        Vector3 potentialNewVertex3 = new Vector3();

        for(int x = 0; x < 20; x++)
        {
            // calculate the points halfway on the edges of a triangle and returns them in potentialNewVertex's
            CreateSubTriangle(vertices[baseIcosehedronTriangleIndexOrder[x*3]], vertices[baseIcosehedronTriangleIndexOrder[x*3 + 1]], vertices[baseIcosehedronTriangleIndexOrder[x*3 + 2]], ref potentialNewVertex1, ref potentialNewVertex2, ref potentialNewVertex3);

            // tests if a vertex already exists 
            oldVertexPosition = ExistsYet(potentialNewVertex1, verticesIndex);
            if (oldVertexPosition == -1) // if the vertex doesnt exist we create the vertex
            {
                vertices[verticesIndex] = new Vector3(potentialNewVertex1.x, potentialNewVertex1.y, potentialNewVertex1.z);
                a = verticesIndex++;
            }
            else // else it already exists so we store is so we can add it to vertices[] in the proper order to draw triangles
                a = oldVertexPosition;

            oldVertexPosition = ExistsYet(potentialNewVertex2, verticesIndex);
            if (oldVertexPosition == -1)
            {
                vertices[verticesIndex] = new Vector3(potentialNewVertex2.x, potentialNewVertex2.y, potentialNewVertex2.z);
                b = verticesIndex++;
            }
            else
                b = oldVertexPosition;
            
            oldVertexPosition = ExistsYet(potentialNewVertex3, verticesIndex);
            if (oldVertexPosition == -1)
            {
                vertices[verticesIndex] = new Vector3(potentialNewVertex3.x, potentialNewVertex3.y, potentialNewVertex3.z);
                c = verticesIndex++;
            }
            else
                c = oldVertexPosition;

            triangles.Add(baseIcosehedronTriangleIndexOrder[x*3]); triangles.Add(a); triangles.Add(c);
            triangles.Add(baseIcosehedronTriangleIndexOrder[x*3 + 1]); triangles.Add(b); triangles.Add(a);
            triangles.Add(baseIcosehedronTriangleIndexOrder[x*3 + 2]); triangles.Add(c); triangles.Add(b);
            triangles.Add(a); triangles.Add(b); triangles.Add(c);

        }

    }

    // calculates the point halfway on each triangle edge then projects it out from the center appropriatly
    void CreateSubTriangle(Vector3 point1, Vector3 point2, Vector3 point3, ref Vector3 newPoint1, ref Vector3 newPoint2, ref Vector3 newPoint3)
    {
        float x, y, z, length;
        float displacer = 2.0f;
        float radius = sphereRadius;

        x = (point1.x + point2.x) / 2.0f;
        y = (point1.y + point2.y) / 2.0f;
        z = (point1.z + point2.z) / 2.0f;
        //length = radius*displacer / Mathf.Sqrt(x * x + y * y + z * z);
        length = (radius+1) / Mathf.Sqrt(x * x + y * y + z * z);
        newPoint1.x = x * length; newPoint1.y = y * length; newPoint1.z = z * length;

        x = (point2.x + point3.x) / 2.0f;
        y = (point2.y + point3.y) / 2.0f;
        z = (point2.z + point3.z) / 2.0f;
        //length = radius*displacer / Mathf.Sqrt(x * x + y * y + z * z);
        length = (radius+1) / Mathf.Sqrt(x * x + y * y + z * z);
        newPoint2.x = x * length; newPoint2.y = y  * length; newPoint2.z = z * length;

        x = (point1.x + point3.x) / 2.0f;
        y = (point1.y + point3.y) / 2.0f;
        z = (point1.z + point3.z) / 2.0f;
        //length = radius*displacer / Mathf.Sqrt(x * x + y * y + z * z);
        length = (radius+1) / Mathf.Sqrt(x * x + y * y + z * z);
        newPoint3.x = x * length; newPoint3.y = y * length; newPoint3.z = z * length;
    }

    // tests if a vertex exists yet or not, returns -1 if it doesnt and if it does it returns its position in vertices[]
    int ExistsYet(Vector3 v, int size)
    {
        for(int x = 0; x < size; x++)
        {
            if (v == vertices[x])
                return x;
        }

        return -1;
    }

    // gives info to the mesh renderer to draw the mesh
    void ProduceIcosphere()
    {

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // morphs the Icosphere to look like an asteroid by taking 3 vertices at a time that form a triangle and projects them in or out from the center
    void MorphIcosphere()
    {
        float length;
        float displacer = 2.0f;
        float x, y, z;
        //Vector3 newPosition = new Vector3();

        // dismorph an random amount of triangles between
        // 0 and maxAsteroidNoise
        int chanceOfDismorph = maxAsteroidNoise;
        while(Random.Range(0, chanceOfDismorph + 1) != 0)
        {
            chanceOfDismorph -= 1;

            // pick a random triangle to displace
            // and make sure ran refrences its first
            // of 3 vertices
            // - 4 so we dont pick a vertices at the end of the list so we cant form a complete triangle with
            // + 1 to make inclusive
            int ran = Random.Range(0, 80 - 4 + 1);
            if (ran % 3 == 1)
                ran += 2;
            else if (ran % 3 == 2)
                ran += 1;

            float radius = sphereRadius + Random.Range((-radiusVariationRange * sphereRadius), (radiusVariationRange * sphereRadius + 0.1f));
            for (int i = 0; i < 3; i++) // displaces the vertices associated with a particular triangle
            {
                //radius = sphereRadius + Random.Range(-radiusVariationRange, radiusVariationRange);
                x = vertices[triangles[ran + i]].x;
                y = vertices[triangles[ran + i]].y;
                z = vertices[triangles[ran + i]].z;

                length = radius*displacer / Mathf.Sqrt(x * x + y * y + z * z);
                //length = (radius + 1) / Mathf.Sqrt(x * x + y * y + z * z);
                vertices[triangles[ran + i]].x = x * length; vertices[triangles[ran + i]].y = y * length; vertices[triangles[ran + i]].z = z * length;
                //MakeCubeAtPoint(vertices[triangles[ran + i]]);
            }
        }
    }


    // call this when health <= 0
    public void SubtractHealth(int damage)
    {                            
        health -= damage;

        if (health <= 0)
        {
            if (sphereRadius >= 3.3f) // massive asteroid, no chance of lossed mass upon breaking
            {
                int x = Random.Range(2, 5);
                if(x == 2)
                {
                    float a = Random.Range(sphereRadius/2.0f - 0.75f, sphereRadius/2.0f + 0.75f);
                    produceSmallerAsteroid(a);
                    produceSmallerAsteroid(sphereRadius - a);
                }
                else if (x == 3)
                {
                    float a = Random.Range(sphereRadius/3.0f - 0.3f, sphereRadius/3.0f + 0.3f);
                    float b = Random.Range(sphereRadius/3.0f - 0.3f, sphereRadius/3.0f + 0.3f);
                    produceSmallerAsteroid(a);
                    produceSmallerAsteroid(b);
                    produceSmallerAsteroid(sphereRadius - a - b);
                }
                else
                {
                    float a = Random.Range(sphereRadius/4.5f - 0.2f, sphereRadius/4.5f + 0.2f);
                    float b = Random.Range(sphereRadius/4.5f - 0.2f, sphereRadius/4.5f + 0.2f);
                    produceSmallerAsteroid(a);
                    produceSmallerAsteroid(b);
                    produceSmallerAsteroid(sphereRadius - a);
                    produceSmallerAsteroid(sphereRadius - b);
                }
            }
            if (sphereRadius >= 2.5f) // huge huge asteroid
            {
                if(Random.Range(0, 3 -1) == 0) // break into 2 peices
                {
                    produceSmallerAsteroid(Random.Range(1.0f, 1.25f));
                    produceSmallerAsteroid(Random.Range(1.0f, 1.25f));
                }
                else if(Random.Range(0, 2 -1) == 0) // break into 3 peices
                {
                    produceSmallerAsteroid(Random.Range(0.6f, 0.83f));
                    produceSmallerAsteroid(Random.Range(0.6f, 0.83f));
                    produceSmallerAsteroid(Random.Range(0.6f, 0.83f));
                }
                else // break into 4 peice
                {
                    produceSmallerAsteroid(Random.Range(0.44f, 0.51f));
                    produceSmallerAsteroid(Random.Range(0.44f, 0.51f));
                    produceSmallerAsteroid(Random.Range(0.44f, 0.51f));
                    produceSmallerAsteroid(Random.Range(0.44f, 0.51f));
                    //cube.GetComponent<Renderer>().sharedMaterial.color = Color.blue;
                }
            }
            else if (sphereRadius >= 2.0f) // huge asteroid
            {
                if(Random.Range(0, 3 -1) == 0) // break into 2 peices
                {
                    produceSmallerAsteroid(Random.Range(0.7f, 1.0f));
                    produceSmallerAsteroid(Random.Range(0.7f, 1.0f));
                }
                else if(Random.Range(0, 2 -1) == 0) // break into 3 peices
                {
                    produceSmallerAsteroid(Random.Range(0.45f, 0.65f));
                    produceSmallerAsteroid(Random.Range(0.45f, 0.65f));
                    produceSmallerAsteroid(Random.Range(0.45f, 0.65f));
                }
                else // break into 4 peice
                {
                    produceSmallerAsteroid(Random.Range(0.4f, 0.5f));
                    produceSmallerAsteroid(Random.Range(0.4f, 0.5f));
                    produceSmallerAsteroid(Random.Range(0.4f, 0.5f));
                    produceSmallerAsteroid(Random.Range(0.4f, 0.5f));
                    //cube.GetComponent<Renderer>().sharedMaterial.color =Color.blue;
                }
            }
            else if(sphereRadius >= 1.5f) // big asteroid
            {
                if(Random.Range(0, 3 -1) == 0) // break into 3 peices
                {
                    produceSmallerAsteroid(Random.Range(0.4f, 0.5f));
                    produceSmallerAsteroid(Random.Range(0.4f, 0.5f));
                    produceSmallerAsteroid(Random.Range(0.4f, 0.5f));
                }
                else // break into 2 peices
                {
                    produceSmallerAsteroid(Random.Range(0.5f, 0.7f));
                    produceSmallerAsteroid(Random.Range(0.5f, 0.7f));
                }
            }
            else if (sphereRadius >= 1.0f) // medium asteroid
            {
                if (Random.Range(0, 2 -1) == 0) // break into 2 peices
                {
                    produceSmallerAsteroid(Random.Range(0.3f, 0.5f));
                    produceSmallerAsteroid(Random.Range(0.3f, 0.5f));
                }
                //else
                    //cube.GetComponent<Renderer>().sharedMaterial.color= Color.blue;
            }
            else if (sphereRadius >= 0.6f) // small asteroid
            {
                if (Random.Range(0, 3 -1) == 0) // break into 2 peice
                    produceSmallerAsteroid(0.3f);
                    produceSmallerAsteroid(0.3f);
            }


            Destroy(this.gameObject);
        }

    }

    // produces smaller asteroids with givin radius and some values based off of this asteroid
    //public float nextRadius;
    //public Vector3 nextInititalVelocity;
    void produceSmallerAsteroid(float radius)
    {
        //asteroid.GetComponent<ProceduralAsteroid>().sphereRadius = radius;
        //asteroid.GetComponent<ProceduralAsteroid>().radiusVariationRange = radius / 2.0f;
        //asteroid.GetComponent<ProceduralAsteroid>().materialType = materialType;
    
        //asteroid.GetComponent<ProceduralAsteroid>().initialForwardVelocity = rb.velocity;


        //nextRadius = radius;
        //nextInititalVelocity = rb.velocity;
        transform.parent.GetComponent<AsteroidManager>().InstantiateAsteroid(radius, materialType, rb.velocity, transform.position);
        // pass up these plus material type

    }
}