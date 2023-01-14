using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailPlatform : MonoBehaviour
{

    public float step = .01f;
    public Axis axis;
    public Direction direction;
    public bool isMoving = false;
    public GameObject rail;

    // Start is called before the first frame update
    void Start()
    {

        // default value
        if(axis == null){
            axis = Axis.Y;
        }

        if(direction == null){
            direction = Direction.up;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMoving){
            Move();
        }
      
    }


    public void Move(){
        
        if(axis == Axis.Y){
            if(direction == Direction.up){
                MoveUp(step);
            } else{
                MoveDown(step);
            }
        } else {
            if(direction == Direction.right){
                MoveRight(step);
            } else{
                MoveLeft(step);
            }
        }
    }

    void MoveUp(float percent) {

        //move up using localposition
        float futureY = transform.localPosition.y + step;

        // if reach the rail top
        float railTop = .5f - transform.localScale.y;

        if(futureY > railTop) {
            futureY = railTop;
            direction = Direction.down;
        }

        gameObject.transform.localPosition = new Vector2(transform.localPosition.x, futureY);

    }

    void MoveDown(float percent) {

        //move down using localposition
        float futureY = transform.localPosition.y - step;

        // if reach the rail bottom
        float railBottom = -.5f + transform.localScale.y;

        if(futureY < railBottom) {
            futureY = railBottom;
            direction = Direction.up;
        }

        gameObject.transform.localPosition = new Vector2(transform.localPosition.x, futureY);
     }

    void MoveRight(float percent) {
        
        //move right using localposition
        float futureX = transform.localPosition.x + step;

        // if reach the rail right
        float railRight = .5f - transform.localScale.x;

        if(futureX > railRight) {
            futureX = railRight;
            direction = Direction.left;
        }

        gameObject.transform.localPosition = new Vector2(futureX, transform.localPosition.y);
    
    }

    void MoveLeft(float percent) {

        //move left using localposition
        float futureX = transform.localPosition.x - step;

        // if reach the rail left
        float railLeft = -.5f + transform.localScale.x;

        if(futureX < railLeft) {
            futureX = railLeft;
            direction = Direction.right;
        }

        gameObject.transform.localPosition = new Vector2(futureX, transform.localPosition.y);
    
    }



    void OnCollisionEnter2D(Collision2D collision) {
      
        // link object velocity to platform
        if(collision.transform.parent == null){
            collision.transform.parent = transform;
        }       

    }

    void OnCollisionExit2D(Collision2D collision) {
        
        // unlink object velocity to platform
         if(collision.transform.parent == transform){
            collision.transform.parent = null;
        }
      
    }
}
