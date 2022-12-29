using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Physics {

    public static Vector2 fakeGravitySpeed(Rigidbody2D rgbd, Direction direction, float acceleration, float strength, float maxSpeed, float deltaTime, float distancePercent){
        
      float speed = deltaTime * strength + ((float) Math.Log(distancePercent) * acceleration);
      if(speed > maxSpeed){
         speed = maxSpeed;
      }

        switch (direction) {
            case Direction.up:
               return new Vector2(rgbd.velocity.x, speed);
            case Direction.down:
               return new Vector2(rgbd.velocity.x, -speed);
            case Direction.left:
               return new Vector2(-speed, rgbd.velocity.y);
            case Direction.right:
               return new Vector2(speed, rgbd.velocity.y);
            default: 
                return new Vector2();
        }        
 
    }
}