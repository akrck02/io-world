using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Director : MonoBehaviour
{

    public ButtonToggle railButton;

    public RailPlatform railPlatform;

    // Start is called before the first frame update
    void Start()
    {

        // Get body child
        GameObject body = railButton.transform.Find("body").gameObject;

        ButtonData data = new ButtonDataImpl(body);
        RailPlatformToggleHandler platformUpHandler = new RailPlatformToggleHandler(data);
        platformUpHandler.platform = railPlatform;
        railButton.handler = platformUpHandler;

        Debug.Log("ButtonToggle: " + railButton);
        Debug.Log("ButtonToggle.handler: " + railButton.handler);
        Debug.Log("Button.body: " + body);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
