using UnityEngine;
using System;

public class Geometrics {

    public static readonly float UP = 0;
    public static readonly float LEFT = 90;
    public static readonly float DOWN = 180;
    public static readonly float RIGHT = 270;

    public static readonly float MARGIN = 10;

    public static float PercentualDistance(float objectDistance, float maxDistance){
        float deltaDistance = objectDistance / maxDistance;
        return deltaDistance * 100;
    }
    public static bool IsUp(float rotation) {
        return rotation < LEFT || rotation > RIGHT;
    }

    public static bool IsDown(float rotation) {

        bool isDown = !IsUp(rotation);

        if(rotation < LEFT - MARGIN && rotation > LEFT + MARGIN) {
            isDown = false;
        }

        if(rotation < RIGHT - MARGIN && rotation > RIGHT + MARGIN) {
            isDown = false;
        }

        return isDown;
    }

    public static bool IsRight(float rotation) {
        return rotation > DOWN;
    }
    
    public static bool IsLeft(float rotation) {
        return !IsRight(rotation) && rotation != 0 && rotation != 180;
    }

    /**
    * Get the rotation of a game object in positive degrees
    * @param gameObject The game object to get the rotation from
    * @return The rotation of the game object in positive degrees
    */
    public static float GetLocalRotation(GameObject gameObject) {
        return gameObject.transform.localRotation.eulerAngles.z;
    }

    public static Vector2 CalculateAxis(float rotation) {

        float x = 0;
        float y = 0;

        if(Calc.IsBetween(rotation, -22.5, 22.5)) {
            x = 0;
            y = 1;
        }

        if(Calc.IsBetween(rotation, 22.5, 67.5)) {
            x = -1;
            y = 1;
        }

        if(Calc.IsBetween(rotation, 67.5, 112.5)) {
            x = -1;
            y = 0;
        }

        if(Calc.IsBetween(rotation, 112.5, 157.5)) {
            x = -1;
            y = -1;
        }

        if(Calc.IsBetween(rotation, 157.5, 202.5)) {
            x = 0;
            y = -1;
        }

        if(Calc.IsBetween(rotation, 202.5, 247.5)) {
            x = 1;
            y = -1;
        }

        if(Calc.IsBetween(rotation, 247.5, 292.5)) {
            x = 1;
            y = 0;
        }

        if(Calc.IsBetween(rotation, 292.5, 337.5)) {
            x = 1;
            y = 1;
        }

        return new Vector2(x,y);
    }


    private static bool IsInMargin(float value, float target, float margin) {
        return value < target + margin || value > target - margin;
    }

}