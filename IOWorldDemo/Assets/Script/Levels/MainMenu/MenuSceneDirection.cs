using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Experimental.Rendering.Universal;

public class MenuSceneDirection : MonoBehaviour
{

    public GameObject snail;
    public GameObject logo;
    public GameObject lamp;
    public GameObject demo;

    private float snailAnimationFrecuency = 0.5f;
    private Vector2 snailPosition = new Vector2(0, 0);
    

    // Start is called before the first frame update
    void Start()
    {
        snailPosition = snail.transform.position;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SnailAnimation();
    }

    void SnailAnimation()
    {

        // Move snail to the right asynchronously
        Vector2 newpos = new Vector2(snailPosition.x + 2f, snailPosition.y);

        //Random pause
        int randomPause = Random.Range(0, 100);
        if (randomPause > 75)
        {
            return;
        }

        snail.transform.position = Vector3.MoveTowards(snail.transform.position, newpos, .0015f);
     
    }
}
