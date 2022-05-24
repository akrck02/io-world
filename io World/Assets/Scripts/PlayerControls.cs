using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody2D rigibody;

    public Transform groundCheck;

    public LayerMask groundLayer;

    public float moveSpeed = 8f;

    public float jumpPower = 5f;

    public SpriteRenderer

            body,
            eyes,
            tail1,
            tail2;

    public AudioSource jumpSound;

    public LevelLoader levelLoader;

    private float horizontal;

    /**
    * Update is called once per frame
    */
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rigibody.velocity = new Vector2(horizontal * moveSpeed, rigibody.velocity.y);

        if (horizontal > 0)
        {
            MoveAnimation(true);
        }
        else if (horizontal < 0)
        {
            MoveAnimation(false);
        }
    }

    /**
    * On restart input is detected
    * restarts current scene
    */
    public void Restart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    /**
    * Makes the player jump
    */
    public void Jump(InputAction.CallbackContext context)
    {
        print("jump");
        if (context.performed && IsGrounded())
        {
            rigibody.velocity = new Vector2(rigibody.velocity.x, jumpPower);
            jumpSound.Play();
        }
        if (context.canceled && rigibody.velocity.y > 0)
        {
            rigibody.velocity = new Vector2(rigibody.velocity.x, rigibody.velocity.y / 2);
            jumpSound.Play();
        }
    }

    /**
    * Moves the player
    */
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    /**
    * Performs move animation
    */
    void MoveAnimation(bool right)
    {
        float eyesX = eyes.transform.position.x;
        float tail1X = tail1.transform.position.x;
        float tail2X = tail2.transform.position.x;


        if (right)
        {
            eyesX += .1f;
            tail1X -= .05f;
            tail2X -= .1f;
        }
        else
        {
            eyesX -= .1f;
            tail1X += .05f;
            tail2X += .1f;
        }

        if (eyesX > body.transform.position.x + .13f) 
            eyesX = body.transform.position.x + .13f;

        if (tail1X < body.transform.position.x -.05f) 
            tail1X = body.transform.position.x -.05f;

        if (tail2X < body.transform.position.x -.15f) 
            tail2X = body.transform.position.x -.15f;

        if (eyesX < body.transform.position.x - .13f) 
            eyesX = body.transform.position.x - .13f;

        if (tail1X > body.transform.position.x +.05f) 
            tail1X = body.transform.position.x +.05f;

        if (tail2X > body.transform.position.x +.15f) 
            tail2X = body.transform.position.x +.15f;

        Vector3 eyesTarget =
            new Vector3(eyesX,
                body.transform.position.y,
                body.transform.position.z - 1);
        Vector3 Tail1Target =
            new Vector3(tail1X,
                body.transform.position.y,
                body.transform.position.z);
        Vector3 Tail2Target =
            new Vector3(tail2X,
                body.transform.position.y,
                body.transform.position.z);

        eyes.transform.position =
            Vector2
                .MoveTowards(eyes.transform.position,
                eyesTarget,
                1 * Time.deltaTime);

        tail1.transform.position =
            Vector2
                .MoveTowards(tail1.transform.position,
                Tail1Target,
                1 * Time.deltaTime);

        tail2.transform.position =
            Vector2
                .MoveTowards(tail2.transform.position,
                Tail2Target,
                1 * Time.deltaTime);
    }

    /**
    * On Collision is detected with the end of the level
    * loads the next scene
    */
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "end")
        {
            levelLoader.LoadNextLevel();
        }
    }

    /**
    * Gets if player is grounded
    */
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
