using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] Vector3 moveVal;
    [SerializeField] float moveSpeed;

    public SpriteRenderer body, eyes, tail1, tail2;
    public Rigidbody2D rigibody;
    public bool grounded;
    public AudioSource jumpSound;
    public LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        this.moveSpeed = 15f;
        this.grounded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Detect witch orientation the player is facing
        if (moveVal.x < 0)
        {
            MoveLeft();  
        }
        else if (moveVal.x > 0)
        {
            MoveRight();
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        rigibody.AddForce(new Vector2(moveVal.x, 0) * moveSpeed);
        
    }
 
    void OnRestart()
    {
        print("Restart");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void MoveLeft(){
       
        Vector3 eyesTarget = new Vector3(body.transform.position.x - .1f, body.transform.position.y, body.transform.position.z); 
        Vector3 Tail1Target = new Vector3(body.transform.position.x + .05f, body.transform.position.y, body.transform.position.z); 
        Vector3 Tail2Target = new Vector3(body.transform.position.x + .1f, body.transform.position.y, body.transform.position.z); 

        eyes.transform.position = Vector2.MoveTowards(
            eyes.transform.position,
            eyesTarget,
            1 * Time.deltaTime
        );

        tail1.transform.position = Vector2.MoveTowards(
            tail1.transform.position,
            Tail1Target,
            1 * Time.deltaTime
        );
        
       tail2.transform.position = Vector2.MoveTowards(
            tail2.transform.position,
            Tail2Target,
            1 * Time.deltaTime
        );
        

    }

    void MoveRight(){
         
        Vector3 eyesTarget = new Vector3(body.transform.position.x + .1f, body.transform.position.y, body.transform.position.z); 
        Vector3 Tail1Target = new Vector3(body.transform.position.x - .05f, body.transform.position.y, body.transform.position.z); 
        Vector3 Tail2Target = new Vector3(body.transform.position.x - .1f, body.transform.position.y, body.transform.position.z); 

        eyes.transform.position = Vector2.MoveTowards(
            eyes.transform.position,
            eyesTarget,
            1 * Time.deltaTime
        );

        tail1.transform.position = Vector2.MoveTowards(
            tail1.transform.position,
            Tail1Target,
            1 * Time.deltaTime
        );
        
       tail2.transform.position = Vector2.MoveTowards(
            tail2.transform.position,
            Tail2Target,
            1 * Time.deltaTime
        );
    }

    void OnJump(){
        moveVal.y = 30f;
        this.grounded = false;
    
        jumpSound.Play();
        rigibody.AddForce(new Vector2(0, moveVal.y) * moveSpeed);

    }


  void OnCollisionEnter2D(Collision2D coll)
    {
        print(coll.gameObject.tag);

        if (coll.gameObject.tag == "end")
        {
            levelLoader.LoadNextLevel();
        }
    }

}
