using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePlayer : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player.transform.position.Set(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
