using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMode : MonoBehaviour
{

    public bool active = false;

    public void toggle()
    {
        active = !active;
    }

    public void activate()
    {
        this.active = true;
    }


    public bool isActive()
    {
        return active;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
