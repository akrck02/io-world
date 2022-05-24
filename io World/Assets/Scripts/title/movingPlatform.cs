using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public GameObject platform;

    public float moveSpeed;

    public float maxY;

    public float minY;

    private Vector3 target;

    private bool lift = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update(){}

    void FixedUpdate()
    {
        if (!lift)
             target = new Vector3(platform.transform.position.x, this.minY, platform.transform.position.z);
        else 
            target = new Vector3(platform.transform.position.x, this.maxY, platform.transform.position.z);


        //if the plaftform has reached the target, reverse the direction
        platform.transform.position = Vector2.MoveTowards(platform.transform.position, target, moveSpeed * Time.deltaTime); 
        if (platform.transform.position.y == target.y) lift = !lift;
        
    }
}
