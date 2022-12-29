using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eject : MonoBehaviour
{

    private Rigidbody2D ownRgbd;
    

    // Start is called before the first frame update
    void Start()
    {
        this.ownRgbd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
