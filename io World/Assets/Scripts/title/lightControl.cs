using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class lightControl : MonoBehaviour
{
    public GameObject button;

    public Light2D light;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        print(coll.gameObject.name);

        if (coll.gameObject.name == "button")
        {
            buttonMode mode = button.GetComponent<buttonMode>();
            mode.activate();

            if (!mode.active)
            {
                light.intensity = 0.2f;
            }
            else
            {
                light.intensity = 1;
            }
        }
    }
}
