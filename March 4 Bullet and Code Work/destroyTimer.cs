using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyTimer : MonoBehaviour
{
    public float destCall;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("timedDestroy", destCall);
    }

    // Update is called once per frame
    void timedDestroy()
    {
        Destroy(gameObject);
    }
}
