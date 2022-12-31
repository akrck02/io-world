using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailPlatformToggleHandler : IButtonToggleHandler {

    private ButtonData bodyData;
    public RailPlatform platform;

  
    public RailPlatformToggleHandler(ButtonData bodyData) {
        this.bodyData = bodyData;


    }


    public void on() {
        platform.isMoving = true;

        //change color to red
        bodyData.SpriteRenderer.color = new Color(255f/255f, 81f/255f, 81f/255f);

        // move body down
       bodyData.Transform.position = new Vector3(bodyData.Transform.position.x, bodyData.Transform.position.y - 0.1f, bodyData.Transform.position.z);    
        
    }

    public void off() {
        platform.isMoving = false;

        // change color to 7DBCCA
        bodyData.SpriteRenderer.color = new Color(125f/255f, 188f/255f, 202f/255f);
      
        // move body up
        bodyData.Transform.position = new Vector3(bodyData.Transform.position.x, bodyData.Transform.position.y + 0.1f, bodyData.Transform.position.z);
      
    }
}
